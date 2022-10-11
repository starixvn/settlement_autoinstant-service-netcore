namespace AutoInstant.Services;

public interface AutoInstantController
{
    Task ProcessAsync();
}

public class AutoInstantControllerImp : AutoInstantController
{
    private readonly MatchSettlingPoolService matchSettlingPoolService;

    private readonly MatchSettleController matchSettleController;

    public AutoInstantControllerImp(
        MatchSettlingPoolService matchSettlingPoolService,
        MatchSettleController matchSettleService)
    {
        this.matchSettlingPoolService = matchSettlingPoolService;
        this.matchSettleController = matchSettleService;
    }

    public async Task ProcessAsync()
    {
        var newMatches = (await matchSettlingPoolService.PeekNewMatch()).Where(n => !n.IsChecking).ToList();

        foreach (var match in newMatches)
        {
            await matchSettleController.Process(match).ConfigureAwait(false);
        }
    }
}