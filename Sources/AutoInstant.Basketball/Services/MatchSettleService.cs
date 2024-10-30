using AutoInstant.Basketball.Constants;
using AutoInstant.Basketball.Criteria;
using AutoInstant.Basketball.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;

namespace AutoInstant.Basketball.Services;

public interface MatchSettleService
{
    Task Settle(Match match);

    Task HandleScoreFailed(Match match);
}

public class MatchSettleServiceImp(
    IDynamicRepository repository,
    IOptions<AppSettingsBasketballKey> appSettingsKey,
    LogService logService,
    GenerateService generateService)
    : MatchSettleService
{
    public async Task Settle(Match match)
    {
        var scoresValid = match.ResultDetails.Where(r => !r.Invalid).ToList();

        if (scoresValid.Count > 0)
        {
            var resource = generateService.CreateResources(scoresValid);
            var determinedType = generateService.GetDeterminedType(scoresValid);

            await UpdateResourceStatus(match.MatchId, resource, Status.Waiting);

            var criteria = new SettleProfileCriteria
            {
                MatchId = match.MatchId,
                SelectedResources = resource,
                DeterminedType = determinedType
            };

            logService.WriteDirectlyLog(criteria);

            await Execute(criteria);

            await UpdateResourceStatus(match.MatchId, resource, Status.Success);
        }
    }

    public async Task HandleScoreFailed(Match match)
    {
        var scoresInvalid = match.ResultDetails.Where(r => r.Invalid).ToList();

        if (scoresInvalid.Count > 0)
        {
            var resource = generateService.CreateResources(scoresInvalid);

            await UpdateResourceStatus(match.MatchId, resource, Status.Failure);

            await logService.WriteLog(match.MatchId, ErrorMessage.ScoreDecreased);
        }
    }

    private async Task Execute(SettleProfileCriteria criteria)
    {
        var servers = appSettingsKey.Value.SiteDbServer;
        var tasks = servers.Select(server => ExecuteOneSite(criteria, server)).ToArray();
        await Task.WhenAll(tasks);
    }

    private async Task ExecuteOneSite(SettleProfileCriteria criteria, string server)
    {
        criteria.DbServer = server;
        await repository.ExecuteAsync(criteria);
    }

    private async Task UpdateResourceStatus(int matchId, string resource, int status)
    {
        var criteria = new UpdateMatchCriteria { MatchId = matchId, SelectedResources = resource, StatusId = status };

        await repository.ExecuteAsync(criteria);
    }
}