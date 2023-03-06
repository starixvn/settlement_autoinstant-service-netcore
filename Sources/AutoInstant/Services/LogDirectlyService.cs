using AutoInstant.Criteria;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface LogDirectlyService
{
    Task WriteLog(Match match, MatchChecking matchChecking);
}

public class LogDirectlyServiceImp : LogDirectlyService
{
    private readonly IDynamicRepository repository;

    public LogDirectlyServiceImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task WriteLog(Match match, MatchChecking matchChecking)
    {
        var criteria = new LogDirectlyCriteria
        {
            Extend = match.MatchId,
            Doing = ""
        };

        await repository.ExecuteAsync(criteria);
    }

    private string GenerateLogData(Match match, MatchChecking matchChecking)
    {
        //var scoreInfo = match.TypeId == Se
        var message = $"NAP:Calculate(CAL) for matchid: {match.MatchId} score: ${match.HomeValue}-{match.AwayValue} HT: ${match.HomeValue}-{match.AwayValue} GoalRecord: {matchChecking.GoalRecord} Remark({matchChecking.BetStatus})";

        return message;
    }
}
