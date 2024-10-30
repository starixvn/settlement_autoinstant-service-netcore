using AutoInstant.Models;
using MatchType = AutoInstant.Constants.MatchType;

namespace AutoInstant.Services;

public interface MatchSettleController
{
    Task Process(Match match);
}

public class MatchSettleControllerImp(
    MatchSettleService matchSettleService,
    ScoreRetryService scoreRetryService,
    MatchSettlingPoolService matchSettlingPoolService,
    MatchQueryService matchQueryService)
    : MatchSettleController
{
    public async Task Process(Match match)
    {
        try
        {
            match.IsChecking = true;
            var isScoreMatch = MatchType.Corner == match.ChildMatchType || await scoreRetryService.IsScoreMatch(match);
            var matchCheckingInfo = await matchQueryService.GetMatchInfo(match);
            if (isScoreMatch)
            {
                await matchSettleService.ScoreMatched(match, matchCheckingInfo);
            }
            else
            {
                await matchSettleService.ScoreMismatched(match, matchCheckingInfo);
            }
        }
        finally
        {
            matchSettlingPoolService.Remove(match);
        }
    }
}