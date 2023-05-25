using AutoInstant.Constants;
using AutoInstant.Models;

namespace AutoInstant.Criteria;

public class ShowLogHalftimeCriteria : LogCriteria
{
    public ShowLogHalftimeCriteria(Match match, MatchInfo matchChecking) : base(match)
    {
        ActionId = SettleAction.ShowReferenceActionsForFirstHalf;
        HTHomeScore = match.HomeValue;
        HTAwayScore = match.AwayValue;
        GoalRecord = matchChecking.GoalRecord;
    }

    public int? HTHomeScore { get; set; }

    public int? HTAwayScore { get; set; }
}