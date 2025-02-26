using AutoInstant.Exceptions;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface ValidationService
{
    Task VerifyMatchBeforeExecution(Match match, MatchInfo matchChecking);

    Task VerifyMatchAfterComparing(Match match, MatchInfo matchChecking);
}

public class ValidationServiceImp(
    ValidationScoreChangeService validationScoreChangeService,
    ValidationSettingMatchService validationSettingMatchService,
    ValidationStatusMatchService validationStatusMatchService,
    ValidateHoldStatusService validateHoldStatusService)
    : ValidationService
{
    public async Task VerifyMatchBeforeExecution(Match match, MatchInfo matchChecking)
    {
        await VerifyMatch(match, matchChecking);
    }

    public async Task VerifyMatchAfterComparing(Match match, MatchInfo matchChecking)
    {
        await VerifyMatch(match, matchChecking);

        throw new ScoreMismatchedException();
    }

    private async Task VerifyMatch(Match match, MatchInfo matchChecking)
    {
        validationSettingMatchService.VerifySettingMatch(matchChecking);

        validationStatusMatchService.VerifyStatusMatch(match, matchChecking);

        validationScoreChangeService.VerifyScoreChanged(match, matchChecking);

        await validateHoldStatusService.ValidateHoldStatus(match);
    }
}