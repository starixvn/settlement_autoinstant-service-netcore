using AutoInstant.Exceptions;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface ValidationService
{
    void VerifyMatchBeforeExecution(Match match, MatchChecking matchChecking);

    void VerifyMatchAfterComparing(Match match, MatchChecking matchChecking);
}

public class ValidationServiceImp : ValidationService
{
    private readonly ValidationScoreChangeService validationScoreChangeService;

    private readonly ValidationSettingMatchService validationSettingMatchService;

    private readonly ValidationStatusMatchService validationStatusMatchService;

    public ValidationServiceImp(
        ValidationScoreChangeService validationScoreChangeService,
        ValidationSettingMatchService validationSettingMatchService,
        ValidationStatusMatchService validationStatusMatchService)
    {
        this.validationScoreChangeService = validationScoreChangeService;
        this.validationSettingMatchService = validationSettingMatchService;
        this.validationStatusMatchService = validationStatusMatchService;
    }

    public void VerifyMatchBeforeExecution(Match match, MatchChecking matchChecking)
    {
        VerifyMatch(match, matchChecking);
    }

    public void VerifyMatchAfterComparing(Match match, MatchChecking matchChecking)
    {
        VerifyMatch(match, matchChecking);

        throw new ScoreMismatchedException();
    }

    private void VerifyMatch(Match match, MatchChecking matchChecking)
    {
        validationSettingMatchService.VerifySettingMatch(matchChecking);

        validationStatusMatchService.VerifyStatusMatch(match, matchChecking);

        validationScoreChangeService.VerifyScoreChanged(match, matchChecking);
    }
}