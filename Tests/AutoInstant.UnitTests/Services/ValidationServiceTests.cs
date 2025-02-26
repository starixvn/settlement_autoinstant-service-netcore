using AutoInstant.Exceptions;
using AutoInstant.Models;
using AutoInstant.Services;

namespace AutoInstant.UnitTests.Services;

public class ValidationServiceTests
{
    private readonly ValidationScoreChangeService _validationScoreChangeService;
    private readonly ValidationSettingMatchService _validationSettingMatchService;
    private readonly ValidationStatusMatchService _validationStatusMatchService;
    private readonly ValidateHoldStatusService _validateHoldStatusService;
    private readonly ValidationServiceImp _service;

    public ValidationServiceTests()
    {
        _validationScoreChangeService = Substitute.For<ValidationScoreChangeService>();
        _validationSettingMatchService = Substitute.For<ValidationSettingMatchService>();
        _validationStatusMatchService = Substitute.For<ValidationStatusMatchService>();
        _validateHoldStatusService = Substitute.For<ValidateHoldStatusService>();
        _service = new ValidationServiceImp(_validationScoreChangeService, _validationSettingMatchService, _validationStatusMatchService, _validateHoldStatusService);
    }

    [Fact]
    public async Task VerifyMatchBeforeExecution_NotThrowException()
    {
        // Assert
        var exception = await Record.ExceptionAsync(() => _service.VerifyMatchBeforeExecution(new Match(), new MatchInfo()));
        Assert.Null(exception);
    }

    [Fact]
    public void VerifyMatchBeforeExecution_InvokeRightOrderService()
    {
        // Act
        _ = _service.VerifyMatchBeforeExecution(new Match(), new MatchInfo());

        // Assert
        Received.InOrder(() =>
        {
            _validationSettingMatchService.Received().VerifySettingMatch(Arg.Any<MatchInfo>());
            _validationStatusMatchService.Received().VerifyStatusMatch(Arg.Any<Match>(), Arg.Any<MatchInfo>());
            _validationScoreChangeService.Received().VerifyScoreChanged(Arg.Any<Match>(), Arg.Any<MatchInfo>());
            _validateHoldStatusService.Received().ValidateHoldStatus(Arg.Any<Match>());
        });
    }

    [Fact]
    public async Task VerifyMatchAfterComparing_ThrowScoreMismatchedException()
    {
        // Assert
        await Assert.ThrowsAsync<ScoreMismatchedException>(() => _service.VerifyMatchAfterComparing(new Match(), new MatchInfo()));
    }
}
