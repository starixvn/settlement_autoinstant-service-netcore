namespace AutoInstant.Basketball.Services;

public interface AutoInstantBasketballController
{
    public Task ProcessAsync();
}

public class AutoInstantBasketballControllerImp : AutoInstantBasketballController
{
    private readonly MatchSettlingPoolService matchSettlingPoolService;
    private readonly SettleMatchService settleMatchService;

    public AutoInstantBasketballControllerImp(
        MatchSettlingPoolService matchSettlingPoolService,
        SettleMatchService settleMatchService)
    {
        this.matchSettlingPoolService = matchSettlingPoolService;
        this.settleMatchService = settleMatchService;
    }

    public async Task ProcessAsync()
    {
        var newMatches = await matchSettlingPoolService.PeekNewMatch();

        foreach (var item in newMatches)
        {
            await settleMatchService.Process(item);
        }

        //newMatches.ForEach(m =>
        //{
        //    Task.Run(() => settleMatchService.Process(m))
        //        .ConfigureAwait(false);
        //});
    }
}