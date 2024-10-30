using AutoInstant.Basketball.Models;

namespace AutoInstant.Basketball.Services;

public interface MatchSettlingPoolService
{
    Task<List<Match>> PeekNewMatch();

    void Remove(Match match);
}

public class MatchSettlingPoolServiceImp(GetMatchService getMatchService) : MatchSettlingPoolService
{
    private readonly object lockObject = new();

    public List<Match> PoolOfMatches { get; } = new();

    public async Task<List<Match>> PeekNewMatch()
    {
        var matches = await getMatchService.GetMatches();
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