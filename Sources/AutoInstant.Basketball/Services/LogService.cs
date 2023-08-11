using AutoInstant.Basketball.Criteria;
using Fanex.Data.Repository;

namespace AutoInstant.Basketball.Services;

public interface LogService
{
    void WriteDirectlyLog(SettleProfileCriteria criteria);

    Task WriteLog(int matchId, string message);
}

public class LogServiceImp : LogService
{
    private readonly IDynamicRepository repository;

    public LogServiceImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public void WriteDirectlyLog(SettleProfileCriteria settleCriteria)
    {
        try
        {
            var criteria = new LogDirectlyCriteria
            {
                Extend = settleCriteria.MatchId,
                Doing = GenerateLogData(settleCriteria)
            };

            repository.Execute(criteria);
        }
        catch (Exception e)
        {
            Fanex.Logging.LogService.Error("Fanex.AutoInstant.Service(Write Directly):", e);
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