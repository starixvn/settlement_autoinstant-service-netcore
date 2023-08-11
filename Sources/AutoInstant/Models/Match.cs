namespace AutoInstant.Models;

public class Match
{
    public int MatchId { get; set; }

    public int LeagueId { get; set; }

    public int TypeId { get; set; }

    public int HomeValue { get; set; }

    public int AwayValue { get; set; }

    public bool IsChecking { get; set; }

    public int InstantKey { get; set; }

    public int ChildMatchType { get; set; }

    public override bool Equals(object obj) =>
        obj is Match match
        && match.MatchId == MatchId;

    public override int GetHashCode()
    {
        return MatchId;
    }
}