using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using AutoInstant.Result.Models;
using AutoInstant.Result.Services;
using Fanex.Settlement.ScaleOut.Request;
using Fanex.Settlement.ScaleOut.Services;
using Match = AutoInstant.Models.Match;

namespace AutoInstant.Services;

public interface PushingCalculationService
{
    Task<Models.Result> Push(Match match, MatchInfo matchChecking);
}

public class PushingCalculationServiceImp(
    IDynamicRepository repository,
    ResultsService resultsService,
    PushingCalculationCriteriaService pushingCalculationCriteriaService,
    ScaleOutSettleService scaleOutSettleService,
    AutoInstantResultService autoInstantResultService,
    IOptions<AppSettingsKey> appSettingsKey,
    LogDirectlyService logDirectlyService)
    : PushingCalculationService
{
    public async Task<Models.Result> Push(Match match, MatchInfo matchChecking)
    {
        var goal = await GetGoal(matchChecking.GoalRecord, match.MatchId);
        var resourceResult = match.TypeId == SettleType.FirstHalf ? resultsService.HalftimeResults(goal) : resultsService.FullTimeResults(goal);

        var result = await ValidateBeforeExecute(match);

        if (result.ErrorCode == 0)
        {
            await autoInstantResultService.Update(match, false, MatchStatus.Waiting);
            var betStatus = GetBetStatus(goal, matchChecking, match);
            await UpdateMatchInfo(match, resourceResult, betStatus);

            var criteria = pushingCalculationCriteriaService.CreateCriteria(match, resourceResult);
            criteria.GoalRecord = matchChecking.GoalRecord;

            await logDirectlyService.WriteLog(criteria);

            await Execute(criteria);

            await autoInstantResultService.UpdateAutoResultStatus(match, MatchStatus.Completed);
        }

        return result;
    }

    private async Task Execute(PushingCalculationCriteria criteria)
    {
        var server = appSettingsKey.Value.SiteDbServer;
        var tasks = server.Select(s => ExecuteOneSite(criteria, s)).ToArray();
        await Task.WhenAll(tasks);
    }

    private async Task ExecuteOneSite(PushingCalculationCriteria criteria, string server)
    {
        criteria.DbServer = server;
        await repository.ExecuteAsync(criteria);
    }

    private string GetBetStatus(Goal goal, MatchInfo matchChecking, Match match)
    {
        var betStatus = matchChecking.BetStatus ?? string.Empty;
        betStatus += match.TypeId == SettleType.FirstHalf ? pushingCalculationCriteriaService.BetStatusHtFl(matchChecking, goal, betStatus) : string.Empty;
        betStatus += pushingCalculationCriteriaService.BetStatusFl(matchChecking, goal, betStatus);
        return betStatus;
    }

    private async Task<Goal> GetGoal(string goalRecord, int matchId)
    {
        var ticketInfo = await GetTicketInfo(matchId);
        return new Goal
        {
            GoalRecord = goalRecord,
            IsExistFgNg = ticketInfo.IsExistFgNg,
            IsExistHtFgNg = ticketInfo.IsExistHtFgNg,
            IsExistFG = ticketInfo.IsExistFG,
            IsExistHTFG = ticketInfo.IsExistHTFG,
            IsExistCorner = ticketInfo.IsExistCorner,
            IsExistHTCorner = ticketInfo.IsExistHTCorner,
            IsExistFirstCorner = ticketInfo.IsExistFirstCorner,
            IsExistHTFirstCorner = ticketInfo.IsExistHTFirstCorner,
            IsExist2HFirstCorner = ticketInfo.IsExist2HFirstCorner,
        };
    }

    private async Task<MatchTicketChecking> GetTicketInfo(int matchId)
    {
        var listTicketInfo = new List<MatchTicketChecking>();
        var criteria = new MatchTicketCheckingCriteria { MatchId = matchId };
        foreach (var server in appSettingsKey.Value.SiteDbServer)
        {
            criteria.DbServer = server;
            var item = await repository.GetAsync<MatchTicketChecking>(criteria) ?? new MatchTicketChecking();
            listTicketInfo.Add(item);
        }

        return new MatchTicketChecking
        {
            IsExistFgNg = listTicketInfo.Exists(item => item.IsExistFgNg),
            IsExistHtFgNg = listTicketInfo.Exists(item => item.IsExistHtFgNg),
            IsExistFG = listTicketInfo.Exists(item => item.IsExistFG),
            IsExistHTFG = listTicketInfo.Exists(item => item.IsExistHTFG),
            IsExistCorner = listTicketInfo.Exists(item => item.IsExistCorner),
            IsExist2HFirstCorner = listTicketInfo.Exists(item => item.IsExist2HFirstCorner),
            IsExistFirstCorner = listTicketInfo.Exists(item => item.IsExistFirstCorner),
            IsExistHTCorner = listTicketInfo.Exists(item => item.IsExistHTCorner),
            IsExistHTFirstCorner = listTicketInfo.Exists(item => item.IsExistHTFirstCorner),
        };
    }

    private async Task<Models.Result> ValidateBeforeExecute(Match match)
    {
        var request = new EventTicketRequest
        {
            MatchId = match.MatchId,
            Action = $"CAL-{Mode(match.TypeId)}",
            SpName = "NAP_Admin_Settlement_InstantCalculation_Push",
            IsAuto = true,
            Mode = null,
            InstantKey = match.InstantKey
        };
        var resultMessage = await scaleOutSettleService.ValidateEventAndTicket(request);

        return new Models.Result
        {
            ErrorCode = resultMessage.ErrorCode,
            ErrorMessage = resultMessage.ErrorMessage
        };
    }

    private static string Mode(int typeId) => typeId == SettleType.FirstHalf ? ModeType.FirstHalf : ModeType.SecondHalf;

    private async Task UpdateMatchInfo(Match match, string resourceResult, string betStatus)
    {
        var requestUpdateInfo = new UpdateMatchInfoRequest
        {
            MatchId = match.MatchId,
            Action = $"CAL-{Mode(match.TypeId)}",
            AdminId = 0,
            ResourceResult = resourceResult,
            IsAuto = true,
            Mode = null,
            BetStatus = betStatus
        };

        if (match.TypeId == SettleType.FirstHalf)
        {
            requestUpdateInfo.HalfTimeHome = match.HomeValue;
            requestUpdateInfo.HalfTimeAway = match.AwayValue;
        }
        else
        {
            requestUpdateInfo.FullTimeHome = match.HomeValue;
            requestUpdateInfo.FullTimeAway = match.AwayValue;
        }

        await scaleOutSettleService.UpdateMatchInfo(requestUpdateInfo);
    }
}