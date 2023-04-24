using AutoInstant.Exceptions;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface MatchSettleService
{
	Task ScoreMatched(Match match, MatchInfo matchInfo);

	void ScoreMismatched(Match match, MatchInfo matchInfo);
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

	public async Task ScoreMatched(Match match, MatchInfo matchInfo)
	{
		try
		{
			validationService.VerifyMatchBeforeExecution(match, matchInfo);
			var result = await pushingCalculationService.Push(match, matchInfo);

			if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
			{
				await logService.ActionLog(match, matchInfo, result.ErrorMessage);
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

	public void ScoreMismatched(Match match, MatchInfo matchInfo)
	{
		try
		{
			validationService.VerifyMatchAfterComparing(match, matchInfo);
		}
		catch (AutoInstantException exception)
		{
			logService.ActionLog(match, matchInfo, exception);
			autoInstantResultService.Insert(match, matchInfo);
			notificationService.AddNotification(match, exception.IsSendToLetsTalk, exception.Description);
		}
	}
}