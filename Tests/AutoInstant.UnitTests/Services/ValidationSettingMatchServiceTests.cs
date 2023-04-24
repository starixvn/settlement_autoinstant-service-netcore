using AutoInstant.Exceptions;
using AutoInstant.Models;
using AutoInstant.Services;

namespace AutoInstant.UnitTests.Services;

public class ValidationSettingMatchServiceTests
{
    private const byte Instant = 2;

    private static readonly byte OffSettingStatus = 1;

    private readonly ValidationSettingMatchService service;

    public ValidationSettingMatchServiceTests()
    {
        service = new ValidationSettingMatchServiceImp();
    }

    [Fact]
    public void VerifySettingMatch_SettingOnAndUndecidedPendingOff_NotThrowException()
    {
        // Arrange
        var matchChecking = new MatchInfo
        {
            AutoSetting = Instant,
            SettlementStatus = 0
        };

        // Assert
        var exception = Record.Exception(() => service.VerifySettingMatch(matchChecking));
        Assert.Null(exception);
    }

    [Fact]
    public void VerifySettingMatch_MatchIsSettingOff_ThrowException()
    {
        // Arrange
        var matchChecking = new MatchInfo
        {
            AutoSetting = OffSettingStatus
        };

        // Assert
        Assert.Throws<SettingOffException>(() => service.VerifySettingMatch(matchChecking));
    }

    [Fact]
    public void VerifySettingMatch_MatchIsMtdHold_ThrowException()
    {
        // Arrange
        var matchChecking = new MatchInfo
        {
            AutoSetting = Instant,
            SettlementStatus = 16
        };

        // Assert
        Assert.Throws<RemarkException>(() => service.VerifySettingMatch(matchChecking));
    }

    [Fact]
    public void VerifySettingMatch_MatchIsUndecided_ThrowException()
    {
        // Arrange
        var matchChecking = new MatchInfo
        {
            AutoSetting = Instant,
            SettlementStatus = 64
        };

        // Assert
        Assert.Throws<RemarkException>(() => service.VerifySettingMatch(matchChecking));
    }

    [Fact]
    public void VerifySettingMatch_MatchIsPending_ThrowException()
    {
        // Arrange
        var matchChecking = new MatchInfo
        {
            AutoSetting = Instant,
            SettlementStatus = 1
        };

        // Assert
        Assert.Throws<RemarkException>(() => service.VerifySettingMatch(matchChecking));
    }

    [Fact]
    public void VerifySettingMatch_MatchIsHoldKey_ThrowException()
    {
        // Arrange
        var matchChecking = new MatchInfo
        {
            AutoSetting = Instant,
            SettlementStatus = 2
        };

        // Assert
        Assert.Throws<RemarkException>(() => service.VerifySettingMatch(matchChecking));
    }
}