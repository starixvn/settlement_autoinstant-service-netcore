using AutoInstant.Exceptions;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface LogService
{
    Task ActionLog(Match match, MatchInfo matchChecking, Models.Result resultValidate);

    Task ActionLog(Match match, MatchInfo matchChecking, AutoInstantException exception);
}

public class LogServiceImp : LogService
{
    private readonly IDynamicRepository repository;

    private readonly LogCriteriaService logCriteriaService;

    private const int ErrorNeedToShow = 1001003;

    public LogServiceImp(
        IDynamicRepository repository,
        LogCriteriaService logCriteriaService)
    {
        this.repository = repository;
        this.logCriteriaService = logCriteriaService;
    }

    public async Task ActionLog(Match match, MatchInfo matchChecking, Models.Result resultValidate)
    {
        var criteria = resultValidate.ErrorCode == ErrorNeedToShow ? logCriteriaService.ShowCreateCriteria(match, matchChecking) : logCriteriaService.HideCreateCriteria(match, matchChecking);
        criteria.Remark = resultValidate.ErrorMessage;
        await repository.ExecuteAsync(criteria);
    }

    public async Task ActionLog(Match match, MatchInfo matchChecking, AutoInstantException exception)
    {
        var criteria = exception.IsShowOnReferenceActions
            ? logCriteriaService.ShowCreateCriteria(match, matchChecking)
            : logCriteriaService.HideCreateCriteria(match, matchChecking);

        criteria.Remark = exception.Description;

        await repository.ExecuteAsync(criteria);
    }
}