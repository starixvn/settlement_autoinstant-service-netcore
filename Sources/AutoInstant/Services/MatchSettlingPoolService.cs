using AutoInstant.Models;

namespace AutoInstant.Services;

public interface MatchSettlingPoolService
{
    Task<List<Match>> PeekNewMatch();

    void Remove(Match match);
}

public class MatchSettlingPoolServiceImp : MatchSettlingPoolService
{
    private readonly MatchByLeagueService matchByLeagueService;

    private readonly object lockObject;

    public MatchSettlingPoolServiceImp(MatchByLeagueService matchByLeagueService)
    {
        this.matchByLeagueService = matchByLeagueService;

        PoolOfMatches = new List<Match>();
        lockObject = new object();
    }

    public List<Match> PoolOfMatches { get; }

    public async Task<List<Match>> PeekNewMatch()
    {
        var matches = await matchByLeagueService.GetMatches();
        lock (lockObject)
        {
            var newMatches = matches.Except(PoolOfMatches).ToList();
            PoolOfMatches.AddRange(newMatches);

            return newMatches;
        }
    }

    public void Remove(Match match)
    {
        lock (lockObject)
        {
            PoolOfMatches.RemoveAll(m => m.MatchId == match.MatchId);
        }
    }
}