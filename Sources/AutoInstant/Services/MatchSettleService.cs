using AutoInstant.Exceptions;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface MatchSettleService
{
    Task ScoreMatched(Match match, MatchInfo matchInfo);

    Task ScoreMismatched(Match match, MatchInfo matchInfo);
}

public class MatchSettleServiceImp(
    ValidationService validationService,
    PushingCalculationService pushingCalculationService,
    AutoInstantResultService autoInstantResultService,
    LogService logService,
    NotificationService notificationService)
    : MatchSettleService
{
    public async Task ScoreMatched(Match match, MatchInfo matchInfo)
    {
        try
        {
            validationService.VerifyMatchBeforeExecution(match, matchInfo);
            var result = await pushingCalculationService.Push(match, matchInfo);

            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                await logService.ActionLog(match, matchInfo, result);
                await autoInstantResultService.Insert(match, matchInfo);
            }
        }
        catch (AutoInstantException exception)
        {
            await logService.ActionLog(match, matchInfo, exception);
            await autoInstantResultService.Insert(match, matchInfo);
            await notificationService.AddNotification(match, exception.IsSendToLetsTalk, exception.Description);
        }
    }

    public async Task ScoreMismatched(Match match, MatchInfo matchInfo)
    {
        try
        {
            validationService.VerifyMatchAfterComparing(match, matchInfo);
        }
        catch (AutoInstantException exception)
        {
            await logService.ActionLog(match, matchInfo, exception);
            await autoInstantResultService.Insert(match, matchInfo);
            await notificationService.AddNotification(match, exception.IsSendToLetsTalk, exception.Description);
        }
    }
}