using AutoInstant.Exceptions;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface MatchSettleService
{
    Task ScoreMatched(Match match, MatchChecking matchChecking);

    void ScoreMismatched(Match match, MatchChecking matchChecking);
}

public class MatchSettleServiceImp : MatchSettleService
{
    private readonly ValidationService validationService;

    private readonly PushingCalculationService pushingCalculationService;

    private readonly AutoInstantResultService autoInstantResultService;

    private readonly LogService logService;

    private readonly NotificationService notificationService;

    public MatchSettleServiceImp(
      ValidationService validationService,
      PushingCalculationService pushingCalculationService,
      AutoInstantResultService autoInstantResultService,
      LogService logService,
      NotificationService notificationService)
    {
        this.validationService = validationService;
        this.pushingCalculationService = pushingCalculationService;
        this.autoInstantResultService = autoInstantResultService;
        this.logService = logService;
        this.notificationService = notificationService;
    }

    public async Task ScoreMatched(Match match, MatchChecking matchChecking)
    {
        try
        {
            validationService.VerifyMatchBeforeExecution(match, matchChecking);
            //
            var result = await pushingCalculationService.Push(match, matchChecking);

            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                await logService.ActionLog(match, matchChecking, result.ErrorMessage);
                await autoInstantResultService.Insert(match, matchChecking);
            }
        }
        catch (AutoInstantException exception)
        {
            await logService.ActionLog(match, matchChecking, exception);
            await autoInstantResultService.Insert(match, matchChecking);
            await notificationService.AddNotification(match, exception.IsSendToLetsTalk, exception.Description);
        }
    }

    public void ScoreMismatched(Match match, MatchChecking matchChecking)
    {
        try
        {
            validationService.VerifyMatchAfterComparing(match, matchChecking);
        }
        catch (AutoInstantException exception)
        {
            logService.ActionLog(match, matchChecking, exception);
            autoInstantResultService.Insert(match, matchChecking);
            notificationService.AddNotification(match, exception.IsSendToLetsTalk, exception.Description);
        }
    }
}