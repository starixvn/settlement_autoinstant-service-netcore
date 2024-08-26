namespace AutoInstant.Basketball.Models;

public class Match
{
    public int MatchId { get; set; }
    public int SportType { get; set; }
    public int LeagueId { get; set; }

    public IEnumerable<ResultDetail> ResultDetails { get; set; }

    public override bool Equals(object obj) =>
        obj is Match match
        && match.MatchId == MatchId;

    public override int GetHashCode()
    {
        return MatchId;
    }
}