using AutoInstant.Constants;
using AutoInstant.Exceptions;
using AutoInstant.Models;
using AutoInstant.Services;

namespace AutoInstant.UnitTests.Services;

public class ValidationStatusMatchServiceTests
{
    private readonly ValidationStatusMatchService service;

    public ValidationStatusMatchServiceTests()
    {
        service = new ValidationStatusMatchServiceImp();
    }

    [Fact]
    public void VerifyStatusMatch_NotThrowException()
    {
        // Arrange

        var matchChecking = new MatchInfo
        {
            IsInstCalculated = false,
            IsCalculatedFT = false,
            IsCalculatedHT = true,
        };

        // Assert
        var exception = Record.Exception(() => service.VerifyStatusMatch(new Match(), matchChecking));
        Assert.Null(exception);
    }

    [Fact]
    public void VerifyStatusMatch_MatchIsManualInstCalculated_ThrowException()
    {
        // Arrange
        var match = new Match
        {
            HomeValue = 1,
            AwayValue = 2
        };

        var matchChecking = new MatchInfo
        {
            IsInstCalculated = true,
            PreviousHomeScore = "1",
            PreviousAwayScore = "1"
        };

        // Assert
        Assert.Throws<ManualCalculatedException>(() => service.VerifyStatusMatch(match, matchChecking));
    }

    [Fact]
    public void VerifyStatusMatch_MatchIsCalculatedFT_ThrowException()
    {
        // Arrange
        var matchChecking = new MatchInfo
        {
            IsCalculatedFT = true,
        };

        // Assert
        Assert.Throws<FTCalculatedException>(() => service.VerifyStatusMatch(new Match { TypeId = SettleType.SecondHalf }, matchChecking));
    }

    [Fact]
    public void VerifyStatusMatch_IsHalfTimeNotDone_ThrowException()
    {
        // Arrange
        var matchChecking = new MatchInfo
        {
            IsCalculatedHT = false,
            TypeId = SettleType.SecondHalf
        };

        // Assert
        Assert.Throws<HalfTimeNotDoneException>(() => service.VerifyStatusMatch(new Match(), matchChecking));
    }
}