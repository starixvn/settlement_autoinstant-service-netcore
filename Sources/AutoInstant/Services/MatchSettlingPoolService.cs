using AutoInstant.Models;

namespace AutoInstant.Services;

public interface MatchSettlingPoolService
{
    Task<List<Match>> PeekNewMatch();

    void Remove(Match match);
}

public class MatchSettlingPoolServiceImp(MatchByLeagueService matchByLeagueService) : MatchSettlingPoolService
{
    private readonly object lockObject = new();

    public List<Match> PoolOfMatches { get; } = new();

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