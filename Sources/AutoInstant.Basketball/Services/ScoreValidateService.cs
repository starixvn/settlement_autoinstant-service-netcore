using AutoInstant.Basketball.Criteria;
using AutoInstant.Basketball.Models;
using Fanex.Data.Repository;

namespace AutoInstant.Basketball.Services;

public interface ScoreValidateService
{
    Task IsScoreDecreased(Match match);
}

public class ScoreValidateServiceImp : ScoreValidateService
{
    private readonly IDynamicRepository repository;

    public ScoreValidateServiceImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task IsScoreDecreased(Match match)
    {
        var getLatestScoreSettled = new GetLatestScoreCriteria
        {
            MatchId = match.MatchId,
            ResourceIds = string.Join(',', match.ResultDetails.Select(r => r.ResourceId).ToList())
        };

        var latestScores = (await repository.FetchAsync<Score>(getLatestScoreSettled)).ToList();

        foreach (var resultDetail in match.ResultDetails)
        {
            var score = latestScores.FirstOrDefault(s => s.ResourceId == resultDetail.ResourceId);

            resultDetail.Invalid = score != null &&
                                   (resultDetail.HomeValue < score.HomeValue ||
                                    resultDetail.AwayValue < score.AwayValue);
        }
    }
}