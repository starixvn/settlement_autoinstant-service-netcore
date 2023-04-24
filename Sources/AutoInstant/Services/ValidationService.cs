using AutoInstant.Exceptions;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface ValidationService
{
    void VerifyMatchBeforeExecution(Match match, MatchInfo matchChecking);

    void VerifyMatchAfterComparing(Match match, MatchInfo matchChecking);
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