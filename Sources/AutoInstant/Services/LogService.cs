using AutoInstant.Exceptions;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface LogService
{
    Task ActionLog(Match match, MatchInfo matchChecking, Models.Result resultValidate);

    Task ActionLog(Match match, MatchInfo matchChecking, AutoInstantException exception);
}

public class LogServiceImp(
    IDynamicRepository repository,
    LogCriteriaService logCriteriaService)
    : LogService
{
    private const int ErrorNeedToShow = 1_001_003;

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