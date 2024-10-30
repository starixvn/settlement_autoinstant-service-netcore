using AutoInstant.Basketball.Models;

namespace AutoInstant.Basketball.Services;

public interface SettleMatchService
{
    public Task Process(Match match);
}

public class SettleMatchServiceImp(
    MatchSettlingPoolService matchSettlingPoolService,
    ScoreValidateService scoreValidateService,
    MatchSettleService matchSettleService)
    : SettleMatchService
{
    public async Task Process(Match match)
    {
        try
        {
            await scoreValidateService.ScoreValidation(match);

            await matchSettleService.Settle(match);

            await matchSettleService.HandleScoreFailed(match);
        }
        finally
        {
            matchSettlingPoolService.Remove(match);
        }
    }
}