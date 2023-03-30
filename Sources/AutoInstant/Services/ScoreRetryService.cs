using AutoInstant.Models;

namespace AutoInstant.Services;

public interface ScoreRetryService
{
    Task<bool> IsScoreMatch(Match match);
}

public class ScoreRetryServiceImp : ScoreRetryService
{
    private readonly ScoreMatcherService scoreMatcherService;

    private readonly AppSettingsKey appSettingsKey;

    public ScoreRetryServiceImp(
        ScoreMatcherService scoreMatcherService,
        IOptions<AppSettingsKey> appSettingsKey)
    {
        this.scoreMatcherService = scoreMatcherService;
        this.appSettingsKey = appSettingsKey.Value;
    }

    public async Task<bool> IsScoreMatch(Match match)
    {
        for (var i = 0; i <= appSettingsKey.RetryCount; i++)
        {
            var score = await scoreMatcherService.IsScoreMatch(match);

            if (score)
            {
                return true;
            }

            if (i < appSettingsKey.RetryCount)
            {
                await Task.Delay(appSettingsKey.RetryInterval);
            }
        }

        return false;
    }
}