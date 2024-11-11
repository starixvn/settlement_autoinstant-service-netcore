using AutoInstant.Models;

namespace AutoInstant.Services;

public interface MatchSettleController
{
    Task Process(Match match);
}

public class MatchSettleControllerImp(
    MatchSettleService matchSettleService,
    MatchSettlingPoolService matchSettlingPoolService,
    MatchQueryService matchQueryService)
    : MatchSettleController
{
    public async Task Process(Match match)
    {
        try
        {
            match.IsChecking = true;
            var matchCheckingInfo = await matchQueryService.GetMatchInfo(match);
            await matchSettleService.ScoreMatched(match, matchCheckingInfo);
        }
        finally
        {
            matchSettlingPoolService.Remove(match);
        }
    }
}