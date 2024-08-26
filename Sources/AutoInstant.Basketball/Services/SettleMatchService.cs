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
            await scoreValidateService.ScoreValidation(match);

            await matchSettleService.Settle(match);

            await matchSettleService.HandleScoreFailed(match);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
        finally
        {
            matchSettlingPoolService.Remove(match);
        }
    }
}