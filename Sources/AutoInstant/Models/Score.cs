namespace AutoInstant.Models;

public class Score
{
    public int MatchId { get; init; }

    public int SourceId { get; init; }

    public int TypeId { get; init; }

    public string HomeValue { get; init; }

    public string AwayValue { get; init; }
}