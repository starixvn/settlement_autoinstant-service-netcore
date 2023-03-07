using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using Fanex.Settlement.ScaleOut.Constants;

namespace AutoInstant.Services;

public interface LogDirectlyService
{
    void WriteLog(Match match, MatchChecking matchChecking);
}

public class LogDirectlyServiceImp : LogDirectlyService
{
    private readonly IDynamicRepository repository;

    public LogDirectlyServiceImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public void WriteLog(Match match, MatchChecking matchChecking)
    {
        try
        {
            var criteria = new LogDirectlyCriteria
            {
                Extend = match.MatchId,
                Doing = GenerateLogData(match, matchChecking)
            };

            repository.Execute(criteria);
        }
        catch (Exception e)
        {
            Fanex.Logging.LogService.Error("Fanex.AutoInstant.Service(Write Directly):", e);
        }

    }

    private string GenerateLogData(Match match, MatchChecking matchChecking)
    {
        var scoreInfo = match.TypeId == SettleType.FirstHalf
            ? $"HT: {match.HomeValue}-{match.AwayValue}"
            : $"score: {match.HomeValue}-{match.AwayValue}";

        var actionName = match.TypeId == SettleType.FirstHalf ? ActionName.CalculateHalfTime : ActionName.CalculateFullTime;

        var message = $"NAP:Calculate({actionName}) for matchid: {match.MatchId} {scoreInfo} GoalRecord: {matchChecking.GoalRecord} Remark({matchChecking.BetStatus})";

        return message;
    }
}
