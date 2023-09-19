namespace AutoInstant.Basketball.Models;

public class ResultDetail
{
    public int MatchId { get; set; }

    public int LeagueId { get; set; }

    public int HomeValue { get; set; }

    public int AwayValue { get; set; }

    public string ResourceId { get; set; }

    public bool Invalid { get; set; }
}