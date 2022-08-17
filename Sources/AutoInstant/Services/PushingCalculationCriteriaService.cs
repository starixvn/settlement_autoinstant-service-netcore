using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using AutoInstant.Result.Models;

namespace AutoInstant.Services
{
    public interface PushingCalculationCriteriaService
    {
        PushingCalculationCriteria CreateCriteria(Match match, string resourceResult, string betStatus);

        string BetStatusHTFL(string matchBetStatus, MatchChecking matchChecking, Goal goal);

        string BetStatusFL(string matchBetStatus, MatchChecking matchChecking, Goal goal);
    }

    public class PushingCalculationCriteriaServiceImp : PushingCalculationCriteriaService
    {
        private readonly char HomeValue = 'H';

        private readonly string FGHomeBetStatus = "FL:1-0;";

        private readonly string FGAwayBetStatus = "FL:2-0;";

        private readonly string HTFGHomeBetStatus = "1FL:1-0;";

        private readonly string HTFGAwayBetStatus = "1FL:2-0;";

        private readonly string Fl = "FL";

        private readonly string HalfTimeFL = "1FL";

        public PushingCalculationCriteria CreateCriteria(Match match, string resourceResult, string betStatus)
        {
            return match.TypeId == SettleType.FirstHalf
                ? new PushingCalculationHalftimeCriteria(match, resourceResult, betStatus)
                : new PushingCalculationFullTimeCriteria(match, resourceResult, betStatus);
        }

        public string BetStatusHTFL(string matchBetStatus, MatchChecking matchChecking, Goal goal)
        {
            var result = matchBetStatus ?? string.Empty;
            var indexOfSemicolumm = matchChecking.GoalRecord.IndexOf(";");

            if (indexOfSemicolumm > 0 && !result.Contains(HalfTimeFL) && goal.IsExistsHTFG)
            {
                return matchChecking.GoalRecord[..(indexOfSemicolumm - 1)][0] == HomeValue ? HTFGHomeBetStatus : HTFGAwayBetStatus;
            }

            return string.Empty;
        }

        public string BetStatusFL(string matchBetStatus, MatchChecking matchChecking, Goal goal)
        {
            var betStatus = matchBetStatus ?? string.Empty;
            var indexOf1FL = betStatus.IndexOf(HalfTimeFL);
            var indexOfSemicolumm = matchChecking.GoalRecord.IndexOf(";");
            var betStatusNot1FL = indexOf1FL > 0 ? betStatus.Remove(indexOf1FL, 8) : betStatus;
            if (betStatus.Length > indexOfSemicolumm + 1 && !betStatusNot1FL.Contains(Fl) && goal.IsExistsFG)
            {
                return matchChecking.GoalRecord[(indexOfSemicolumm + 1)..][0] == HomeValue ? FGHomeBetStatus : FGAwayBetStatus;
            }

            return string.Empty;
        }
    }
}