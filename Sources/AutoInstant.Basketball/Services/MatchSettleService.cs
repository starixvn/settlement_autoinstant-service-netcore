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

public class MatchSettleServiceImp : MatchSettleService
{
    private readonly IDynamicRepository repository;

    private readonly AppSettingsBasketballKey appSettingsKey;

    private readonly LogService logService;

    private readonly GenerateService generateService;

    public MatchSettleServiceImp(IDynamicRepository repository,
        IOptions<AppSettingsBasketballKey> appSettingsBasketballKey,
        LogService logService,
        GenerateService generateService)
    {
        this.repository = repository;
        this.appSettingsKey = appSettingsBasketballKey.Value;
        this.logService = logService;
        this.generateService = generateService;
    }

    public async Task Settle(Match match)
    {
        var scoresValid = match.ResultDetails.Where(r => !r.Invalid).ToList();

        if (scoresValid.Count > 0)
        {
            List<string> templateResources = match.SportType == 2
                ? ["01", "NOT_DEFINE", "00", "Q1", "Q2", "Q3", "Q4", "OT"]
                : ["01", "NOT_DEFINE", "00", "0101", "0102", "0103", "0104", "0105", "0106", "0107", "0108", "0109", "EI"];

            var resource = generateService.CreateResources(scoresValid);

            var determinedType = generateService.GetDeterminedType(scoresValid, templateResources);

            await UpdateResourceStatus(match.MatchId, resource, Status.Waitting);

            var criteria = new SettleProfileCriteria
            {
                MatchId = match.MatchId,
                SelectedResources = resource,
                DeterminedType = determinedType
            };

            logService.WriteDirectlyLog(criteria);

            Execute(criteria);
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

    private void Execute(SettleProfileCriteria criteria)
    {
        foreach (var server in appSettingsKey.SiteDbServer)
        {
            criteria.DbServer = server;
            repository.Execute(criteria);
        }
    }

    private async Task UpdateResourceStatus(int matchId, string resource, int status)
    {
        var criteria = new UpdateMatchCriteria { MatchId = matchId, SelectedResources = resource, StatusId = status };

        await repository.ExecuteAsync(criteria);
    }
}