using AutoInstant.Basketball.Constants;
using AutoInstant.Basketball.Criteria;
using AutoInstant.Basketball.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;

namespace AutoInstant.Basketball.Services;

public interface MatchSettleService
{
    Task Settle(int matchId, IReadOnlyList<ResultDetail> resultDetails);

    Task HandleScoreValidateFailed(int matchId, IReadOnlyList<ResultDetail> resultDetails);
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

    public async Task Settle(int matchId, IReadOnlyList<ResultDetail> resultDetails)
    {
        if (!resultDetails.Any())
        {
            return;
        }
        var resource = generateService.CreateResources(resultDetails);
        await UpdateResourceStatus(matchId, resource, Status.Waitting);

        var criteria = new SettleProfileCriteria { MatchId = matchId, SelectedResources = resource, DeterminedType = generateService.GetDeterminedType(resultDetails) };
        logService.WriteDirectlyLog(criteria);

        Execute(criteria);
    }

    public async Task HandleScoreValidateFailed(int matchId, IReadOnlyList<ResultDetail> resultDetails)
    {
        if (!resultDetails.Any())
        {
            return;
        }
        var resource = generateService.CreateResources(resultDetails);
        await UpdateResourceStatus(matchId, resource, Status.Failure);

        await logService.WriteLog(matchId, ErrorMessage.ScoreDecreased);
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