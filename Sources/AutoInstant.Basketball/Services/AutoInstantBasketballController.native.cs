namespace AutoInstant.Basketball.Services;

public interface AutoInstantBasketballController
{
    public Task ProcessAsync();
}

public class AutoInstantBasketballControllerImp(
    MatchSettlingPoolService matchSettlingPoolService,
    SettleMatchService settleMatchService)
    : AutoInstantBasketballController
{
    public async Task ProcessAsync()
    {
        var newMatches = await matchSettlingPoolService.PeekNewMatch();

        newMatches.ForEach(m =>
        {
            Task.Run(() => settleMatchService.Process(m))
                .ConfigureAwait(false);
        });
    }
}