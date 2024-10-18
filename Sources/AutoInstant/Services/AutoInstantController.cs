namespace AutoInstant.Services;

public interface AutoInstantController
{
    Task ProcessAsync();
}

public class AutoInstantControllerImp(
    MatchSettlingPoolService matchSettlingPoolService,
    MatchSettleController matchSettleService)
    : AutoInstantController
{
    public async Task ProcessAsync()
    {
        var newMatches = (await matchSettlingPoolService.PeekNewMatch()).Where(n => !n.IsChecking).ToList();

        newMatches.ForEach(m =>
        {
            Task.Run(() => matchSettleService.Process(m))
                .ConfigureAwait(false);
        });
    }
}