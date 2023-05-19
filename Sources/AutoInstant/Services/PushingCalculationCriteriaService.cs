using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using AutoInstant.Result.Models;

namespace AutoInstant.Services;

public interface PushingCalculationCriteriaService
{
    PushingCalculationCriteria CreateCriteria(Match match, string resourceResult);

    string BetStatusHtFl(MatchInfo matchChecking, Goal goal, string betStatus);

    string BetStatusFl(MatchInfo matchChecking, Goal goal, string betStatus);
}

public class PushingCalculationCriteriaServiceImp : PushingCalculationCriteriaService
{
    private const char HomeValue = 'H';

    private const string FgHomeBetStatus = "FL:1-0;";

    private const string FgAwayBetStatus = "FL:2-0;";

    private const string HtfgHomeBetStatus = "1FL:1-0;";

    private const string HtfgAwayBetStatus = "1FL:2-0;";

    private const string FirstLast = "FL";

    private const string HalfTimeFl = "1FL";

    public PushingCalculationCriteria CreateCriteria(Match match, string resourceResult)
    {
        return match.TypeId == SettleType.FirstHalf
            ? new PushingCalculationHalftimeCriteria(match, resourceResult)
            : new PushingCalculationFullTimeCriteria(match, resourceResult);
    }

    public string BetStatusHtFl(MatchInfo matchChecking, Goal goal, string betStatus)
    {
        var goalRecord = matchChecking.GoalRecord ?? string.Empty;
        var goalOnly = goalRecord.Split(";").First();
        var isNotExistBetStatusHtFl = !betStatus.Contains(HalfTimeFl);

        if (goalOnly.Length > 0 && goal.IsExistHTFG && isNotExistBetStatusHtFl)
        {
            return goalOnly[0] == HomeValue ? HtfgHomeBetStatus : HtfgAwayBetStatus;
        }

        return string.Empty;
    }

    public string BetStatusFl(MatchInfo matchChecking, Goal goal, string betStatus)
    {
        var goalRecord = matchChecking.GoalRecord ?? string.Empty;
        var goalOnly = goalRecord.Replace(";", string.Empty);
        var indexOfHtFl = betStatus.IndexOf(HalfTimeFl, StringComparison.Ordinal);
        var betStatusExcludeHtFl = indexOfHtFl > -1 ? betStatus.Remove(indexOfHtFl, 8) : betStatus;
        var isNotExistBetStatusFl = !betStatusExcludeHtFl.Contains(FirstLast);

        if (goalOnly.Length > 0 && goal.IsExistFG && isNotExistBetStatusFl)
        {
            return goalOnly[0] == HomeValue ? FgHomeBetStatus : FgAwayBetStatus;
        }

        return string.Empty;
    }
}