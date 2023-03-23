using AutoInstant.Criteria;

namespace AutoInstant.Services;

public interface LogDirectlyService
{
    void WriteLog(PushingCalculationCriteria calculationCriteria);
}

public class LogDirectlyServiceImp : LogDirectlyService
{
    private readonly IDynamicRepository repository;

    public LogDirectlyServiceImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public void WriteLog(PushingCalculationCriteria calculationCriteria)
    {
        try
        {
            var criteria = new LogDirectlyCriteria
            {
                Extend = calculationCriteria.MatchId,
                Doing = GenerateLogData(calculationCriteria)
            };

            repository.Execute(criteria);
        }
        catch (Exception e)
        {
            Fanex.Logging.LogService.Error("Fanex.AutoInstant.Service(Write Directly):", e);
        }
    }

    private static string GenerateLogData(PushingCalculationCriteria calculationCriteria)
        => $"NAP: Instant Calculate ({calculationCriteria.Mode}) for matchId: {calculationCriteria.MatchId} "
           + $"Score: {calculationCriteria.HomeScore}-{calculationCriteria.AwayScore} Instant action: {calculationCriteria.Mode}";
}