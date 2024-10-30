using AutoInstant.Basketball.Criteria;
using AutoInstant.Basketball.Models;
using Fanex.Data.Repository;

namespace AutoInstant.Basketball.Services;

public interface ScoreValidateService
{
    Task ScoreValidation(Match match);
}

public class ScoreValidateServiceImp(IDynamicRepository repository) : ScoreValidateService
{
    public async Task ScoreValidation(Match match)
    {
        var criteria = new GetLatestScoreCriteria
        {
            MatchId = match.MatchId,
            ResourceIds = string.Join(',', match.ResultDetails.Select(r => r.ResourceId).ToList())
        };

        var latestScores = (await repository.FetchAsync<Score>(criteria)).ToList();

        foreach (var resultDetail in match.ResultDetails)
        {
            var score = latestScores.Find(s => s.ResourceId == resultDetail.ResourceId);

            resultDetail.Invalid = IsInvalidScore(score, resultDetail);
        }
    }

    private static bool IsInvalidScore(Score score, ResultDetail resultDetail)
    {
        return score != null && (resultDetail.HomeValue < score.HomeValue || resultDetail.AwayValue < score.AwayValue);
    }
}