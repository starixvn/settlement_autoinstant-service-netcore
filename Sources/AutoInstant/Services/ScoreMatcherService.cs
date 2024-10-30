using AutoInstant.Models;

namespace AutoInstant.Services;

public interface ScoreMatcherService
{
    Task<bool> IsScoreMatch(Match match);
}

public class ScoreMatcherServiceImp(ScoreService scoreService) : ScoreMatcherService
{
    public async Task<bool> IsScoreMatch(Match match)
    {
        var scores = await scoreService.GetScore(match.MatchId);

        return scores.Find(x => x.HomeValue == match.HomeValue.ToString()
                                && x.AwayValue == match.AwayValue.ToString()
                                && x.TypeId == match.TypeId) != null;
    }
}