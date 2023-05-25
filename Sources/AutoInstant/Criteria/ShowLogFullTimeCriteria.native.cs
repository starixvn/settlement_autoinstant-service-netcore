using AutoInstant.Constants;
using AutoInstant.Models;

namespace AutoInstant.Criteria;

public class ShowLogFullTimeCriteria : LogCriteria
{
    public ShowLogFullTimeCriteria(Match match, MatchInfo matchChecking) : base(match)
    {
        ActionId = SettleAction.ShowReferenceActionsForSecondHalf;
        FinalHomeScore = match.HomeValue;
        FinalAwayScore = match.AwayValue;
        HTHomeScore = matchChecking.HTHomeScore;
        HTAwayScore = matchChecking.HTAwayScore;
        GoalRecord = matchChecking.GoalRecord;
    }

    public int? HTHomeScore { get; set; }

    public int? HTAwayScore { get; set; }

    public int? FinalHomeScore { get; set; }

    public int? FinalAwayScore { get; set; }
}