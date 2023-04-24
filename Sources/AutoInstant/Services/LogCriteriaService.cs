using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface LogCriteriaService
{
    LogCriteria ShowCreateCriteria(Match match, MatchInfo matchChecking);

    LogCriteria HideCreateCriteria(Match match, MatchInfo matchChecking);
}

public class LogCriteriaServiceImp : LogCriteriaService
{
    public LogCriteria ShowCreateCriteria(Match match, MatchInfo matchChecking)
    {
        return match.TypeId == SettleType.FirstHalf
            ? (LogCriteria)(new ShowLogHalftimeCriteria(match, matchChecking))
            : (LogCriteria)(new ShowLogFullTimeCriteria(match, matchChecking));
    }

    public LogCriteria HideCreateCriteria(Match match, MatchInfo matchChecking)
    {
        return match.TypeId == SettleType.FirstHalf
            ? (LogCriteria)(new HideLogHalftimeCriteria(match, matchChecking))
            : (LogCriteria)(new HideLogFullTimeCriteria(match, matchChecking));
    }
}