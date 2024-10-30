using AutoInstant.Exceptions;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface ValidationService
{
    void VerifyMatchBeforeExecution(Match match, MatchInfo matchChecking);

    void VerifyMatchAfterComparing(Match match, MatchInfo matchChecking);
}

public class ValidationServiceImp(
    ValidationScoreChangeService validationScoreChangeService,
    ValidationSettingMatchService validationSettingMatchService,
    ValidationStatusMatchService validationStatusMatchService)
    : ValidationService
{
    public void VerifyMatchBeforeExecution(Match match, MatchInfo matchChecking)
    {
        VerifyMatch(match, matchChecking);
    }

    public void VerifyMatchAfterComparing(Match match, MatchInfo matchChecking)
    {
        VerifyMatch(match, matchChecking);

        throw new ScoreMismatchedException();
    }

    private void VerifyMatch(Match match, MatchInfo matchChecking)
    {
        validationSettingMatchService.VerifySettingMatch(matchChecking);

        validationStatusMatchService.VerifyStatusMatch(match, matchChecking);

        validationScoreChangeService.VerifyScoreChanged(match, matchChecking);
    }
}