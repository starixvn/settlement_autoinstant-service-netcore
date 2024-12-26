using AutoInstant.Models;

namespace AutoInstant.Services;

public interface ScoreRetryService
{
    Task<bool> IsScoreMatch(Match match);
}

public class ScoreRetryServiceImp(
    ScoreMatcherService scoreMatcherService,
    IOptions<AppSettingsKey> appSettingsKey)
    : ScoreRetryService
{
    public async Task<bool> IsScoreMatch(Match match)
    {
        for (var i = 0; i <= appSettingsKey.Value.RetryCount; i++)
        {
            var score = await scoreMatcherService.IsScoreMatch(match);

            if (score)
            {
                return true;
            }

            if (i < appSettingsKey.Value.RetryCount)
            {
                await Task.Delay(appSettingsKey.Value.RetryInterval);
            }
        }

        return false;
    }
}