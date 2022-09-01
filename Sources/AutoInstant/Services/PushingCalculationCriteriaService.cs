using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using AutoInstant.Result.Models;

namespace AutoInstant.Services
{
    public interface PushingCalculationCriteriaService
    {
        PushingCalculationCriteria CreateCriteria(Match match, string resourceResult);

        string BetStatusHTFL(MatchChecking matchChecking, Goal goal);

        string BetStatusFL(MatchChecking matchChecking, Goal goal);

        string RemoveOldValue(string betStatus);
    }

    public class PushingCalculationCriteriaServiceImp : PushingCalculationCriteriaService
    {
        private readonly char HomeValue = 'H';

        private readonly string FGHomeBetStatus = "FL:1-0;";

        private readonly string FGAwayBetStatus = "FL:2-0;";

        private readonly string HTFGHomeBetStatus = "1FL:1-0;";

        private readonly string HTFGAwayBetStatus = "1FL:2-0;";

        private readonly string Fl = "FL";

        private readonly string HalfTimeFl = "1FL";

        public PushingCalculationCriteria CreateCriteria(Match match, string resourceResult)
        {
            return match.TypeId == SettleType.FirstHalf
                ? new PushingCalculationHalftimeCriteria(match, resourceResult)
                : new PushingCalculationFullTimeCriteria(match, resourceResult);
        }

        public string RemoveOldValue(string betStatus)
        {
            if (betStatus == null)
            {
                return string.Empty;
            }

            betStatus = removeBetStatus(HTFGHomeBetStatus, betStatus);
            betStatus = removeBetStatus(HTFGAwayBetStatus, betStatus);
            betStatus = removeBetStatus(FGHomeBetStatus, betStatus);
            betStatus = removeBetStatus(FGAwayBetStatus, betStatus);

            return betStatus;
        }

        private string removeBetStatus(string key, string betStatus)
        {
            var indexOfKey = betStatus.IndexOf(key);
            if (indexOfKey > -1)
            {
                return betStatus.Remove(indexOfKey, key.Length);
            }

            return betStatus;
        }

        public string BetStatusHTFL(MatchChecking matchChecking, Goal goal)
        {
            var goalOnly = matchChecking.GoalRecord.Replace(";", string.Empty);
            if (goalOnly.Length > 0 && goal.IsExistsHTFG)
            {
                return matchChecking.GoalRecord[0] == HomeValue ? HTFGHomeBetStatus : HTFGAwayBetStatus;
            }

            return string.Empty;
        }

        public string BetStatusFL(MatchChecking matchChecking, Goal goal)
        {
            var goalOnly = matchChecking.GoalRecord.Replace(";", string.Empty);
            if (goalOnly.Length > 0 && goal.IsExistsFG)
            {
                return matchChecking.GoalRecord[0] == HomeValue ? FGHomeBetStatus : FGAwayBetStatus;
            }

            return string.Empty;
        }
    }
}