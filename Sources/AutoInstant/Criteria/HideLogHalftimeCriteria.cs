using AutoInstant.Constants;
using AutoInstant.Models;

namespace AutoInstant.Criteria
{
    public class HideLogHalftimeCriteria : LogCriteria
    {
        public HideLogHalftimeCriteria(Match match, MatchChecking matchChecking) : base(match)
        {
            ActionId = SettleAction.HideReferenceActionsForFirstHalf;
            HTHomeScore = match.HomeValue;
            HTAwayScore = match.AwayValue;
            GoalRecord = matchChecking.GoalRecord;
        }

        public int? HTHomeScore { get; set; }

        public int? HTAwayScore { get; set; }
    }
}