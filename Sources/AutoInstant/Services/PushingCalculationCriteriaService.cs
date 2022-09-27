using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using AutoInstant.Result.Models;
using System;
using System.Linq;

namespace AutoInstant.Services
{
    public interface PushingCalculationCriteriaService
    {
        PushingCalculationCriteria CreateCriteria(Match match, string resourceResult);

        string BetStatusHtFl(MatchChecking matchChecking, Goal goal, string betStatus);

        string BetStatusFl(MatchChecking matchChecking, Goal goal, string betStatus);
    }

    public class PushingCalculationCriteriaServiceImp : PushingCalculationCriteriaService
    {
        private readonly char homeValue = 'H';

        private readonly string fgHomeBetStatus = "FL:1-0;";

        private readonly string fgAwayBetStatus = "FL:2-0;";

        private readonly string htfgHomeBetStatus = "1FL:1-0;";

        private readonly string htfgAwayBetStatus = "1FL:2-0;";

        private readonly string fl = "FL";

        private readonly string halfTimeFl = "1FL";

        public PushingCalculationCriteria CreateCriteria(Match match, string resourceResult)
        {
            return match.TypeId == SettleType.FirstHalf
                ? new PushingCalculationHalftimeCriteria(match, resourceResult)
                : new PushingCalculationFullTimeCriteria(match, resourceResult);
        }

        public string BetStatusHtFl(MatchChecking matchChecking, Goal goal, string betStatus)
        {
            var goalRecord = matchChecking.GoalRecord ?? string.Empty;
            var goalOnly = goalRecord.Split(";").First();
            var isNotExistBetStatusHtFl = !betStatus.Contains(halfTimeFl);
            if (goalOnly.Length > 0 && goal.IsExistHTFG && isNotExistBetStatusHtFl)
            {
                return goalOnly[0] == homeValue ? htfgHomeBetStatus : htfgAwayBetStatus;
            }

            return string.Empty;
        }

        public string BetStatusFl(MatchChecking matchChecking, Goal goal, string betStatus)
        {
            var goalRecord = matchChecking.GoalRecord ?? string.Empty;
            var goalOnly = goalRecord.Replace(";", string.Empty);
            var indexOfHtFl = betStatus.IndexOf(halfTimeFl, StringComparison.Ordinal);
            var betStatusExcludeHtFl = indexOfHtFl > -1 ? betStatus.Remove(indexOfHtFl, 8) : betStatus;
            var isNotExistBetStatusFl = !betStatusExcludeHtFl.Contains(fl);
            if (goalOnly.Length > 0 && goal.IsExistFG && isNotExistBetStatusFl)
            {
                return goalOnly[0] == homeValue ? fgHomeBetStatus : fgAwayBetStatus;
            }

            return string.Empty;
        }
    }
}