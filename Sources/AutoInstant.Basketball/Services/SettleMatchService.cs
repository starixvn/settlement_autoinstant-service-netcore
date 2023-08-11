using AutoInstant.Basketball.Models;

namespace AutoInstant.Basketball.Services;

public interface SettleMatchService
{
    public Task Process(Match match);
}

public class SettleMatchServiceImp : SettleMatchService
{
    private readonly MatchSettlingPoolService matchSettlingPoolService;

    private readonly ScoreValidateService scoreValidateService;

    private readonly MatchSettleService matchSettleService;

    public SettleMatchServiceImp(
        MatchSettlingPoolService matchSettlingPoolService,
        ScoreValidateService scoreValidateService,
        MatchSettleService matchSettleService)
    {
        this.matchSettlingPoolService = matchSettlingPoolService;
        this.scoreValidateService = scoreValidateService;
        this.matchSettleService = matchSettleService;
    }

    public async Task Process(Match match)
    {
        try
        {
            await scoreValidateService.IsScoreDecreased(match);

            await matchSettleService.Settle(match.MatchId, match.ResultDetails.Where(r => !r.Invalid).ToList());

            await matchSettleService.HandleScoreValidateFailed(match.MatchId,
                match.ResultDetails.Where(r => r.Invalid).ToList());
        }
        finally
        {
            matchSettlingPoolService.Remove(match);
        }
    }
}