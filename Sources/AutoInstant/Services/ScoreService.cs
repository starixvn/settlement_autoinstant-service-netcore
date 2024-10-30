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
                         .Where(s => s.SourceId == Source.SoccerWay
                         && (s.TypeId == SettleType.FirstHalf || s.TypeId == SettleType.SecondHalf)).ToList();
    }
}