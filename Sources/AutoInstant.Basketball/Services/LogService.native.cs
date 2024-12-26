using AutoInstant.Basketball.Criteria;
using Fanex.Data.Repository;

namespace AutoInstant.Basketball.Services;

public interface LogService
{
    Task WriteDirectlyLog(SettleProfileCriteria settleProfileCriteria);

    Task WriteLog(int matchId, string message);
}

public class LogServiceImp(IDynamicRepository repository) : LogService
{
    public async Task WriteDirectlyLog(SettleProfileCriteria settleProfileCriteria)
    {
        try
        {
            var criteria = new LogDirectlyCriteria
            {
                Extend = settleProfileCriteria.MatchId,
                Doing = GenerateLogData(settleProfileCriteria)
            };

            await repository.ExecuteAsync(criteria);
        }
        catch (Exception e)
        {
            await Fanex.Logging.LogService.ErrorAsync("Fanex.AutoInstant.Service(Write Directly):", e);
        }
    }

    private static string GenerateLogData(SettleProfileCriteria criteria)
        => $"NAP: Instant Calculate ({criteria.Action}) for matchId: {criteria.MatchId} "
           + $"Result: {criteria.SelectedResources} Instant action: {criteria.Action}";

    public async Task WriteLog(int matchId, string message)
    {
        var criteria = new LogCriteria(matchId, message);
        await repository.ExecuteAsync(criteria);
    }
}