using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface ScoreService
{
    Task<List<Score>> GetScore(int matchId);
}

public class ScoreServiceImp(IDynamicRepository repository) : ScoreService
{
    public async Task<List<Score>> GetScore(int matchId)
    {
        var criteria = new MatchScoreCriteria { MatchId = matchId };
        return (await repository.FetchAsync<Score>(criteria))
                         .Where(s => s.TypeId is SettleType.FirstHalf or SettleType.SecondHalf).ToList();
    }
}