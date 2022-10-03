using AutoInstant.Exceptions;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface LogService
{
    Task ActionLog(Match match, MatchChecking matchChecking, string message);

    Task ActionLog(Match match, MatchChecking matchChecking, AutoInstantException exception);
}

public class LogServiceImp : LogService
{
    private readonly IDynamicRepository repository;

    private readonly LogCriteriaService logCriteriaService;

    public LogServiceImp(
        IDynamicRepository repository,
        LogCriteriaService logCriteriaService)
    {
        this.repository = repository;
        this.logCriteriaService = logCriteriaService;
    }

    public async Task ActionLog(Match match, MatchChecking matchChecking, string message)
    {
        var criteria = logCriteriaService.HideCreateCriteria(match, matchChecking);
        criteria.Remark = message;
        await repository.ExecuteAsync(criteria);
    }

    public async Task ActionLog(Match match, MatchChecking matchChecking, AutoInstantException exception)
    {
        var criteria = exception.IsShowOnReferenceActions
            ? logCriteriaService.ShowCreateCriteria(match, matchChecking)
            : logCriteriaService.HideCreateCriteria(match, matchChecking);

        criteria.Remark = exception.Description;

        await repository.ExecuteAsync(criteria);
    }
}