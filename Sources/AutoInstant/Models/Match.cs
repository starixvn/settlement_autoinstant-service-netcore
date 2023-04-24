namespace AutoInstant.Models;

public class Match
{
	public int MatchId { get; set; }

	public int LeagueId { get; set; }

	public int TypeId { get; set; }

	public int HomeValue { get; set; }

	public int AwayValue { get; set; }
	public string InstantHomeScoreBefore { get; set; }
	public string InstantAwayScoreBefore { get; set; }
	public string InstantGoalRecordBefore { get; set; }

	public bool IsChecking { get; set; }

	public int InstantKey { get; set; }

	public int ChildMatchType { get; set; }

	public override bool Equals(object obj)
	{
		return obj != null
			&& (obj is Match)
			&& (obj as Match).MatchId == MatchId;
	}

	public override int GetHashCode()
	{
		return MatchId;
	}
}