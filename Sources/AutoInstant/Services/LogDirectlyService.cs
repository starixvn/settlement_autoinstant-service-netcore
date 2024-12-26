using AutoInstant.Criteria;

namespace AutoInstant.Services;

public interface LogDirectlyService
{
    Task WriteLog(PushingCalculationCriteria calculationCriteria);
}

public class LogDirectlyServiceImp(IDynamicRepository repository) : LogDirectlyService
{
    public async Task WriteLog(PushingCalculationCriteria calculationCriteria)
    {
        try
        {
            var criteria = new LogDirectlyCriteria
            {
                Extend = calculationCriteria.MatchId,
                Doing = GenerateLogData(calculationCriteria)
            };

            await repository.ExecuteAsync(criteria);
        }
        catch (Exception e)
        {
            await Fanex.Logging.LogService.ErrorAsync("Fanex.AutoInstant.Service(Write Directly):", e);
        }
    }

    private static string GenerateLogData(PushingCalculationCriteria calculationCriteria)
        => $"NAP: Instant Calculate ({calculationCriteria.Action}) for matchId: {calculationCriteria.MatchId} "
           + $"Score: {calculationCriteria.HomeScore}-{calculationCriteria.AwayScore} Instant action: {calculationCriteria.Action}";
}