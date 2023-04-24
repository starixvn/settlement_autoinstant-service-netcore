using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using AutoInstant.Services;

namespace AutoInstant.UnitTests.Services;

public class LogCriteriaServiceTests
{
    private readonly LogCriteriaService service;

    private readonly MatchInfo matchChecking;

    public LogCriteriaServiceTests()
    {
        matchChecking = new MatchInfo();
        service = new LogCriteriaServiceImp();
    }

    [Fact]
    public void CreateCriteria_MatchIsFirstHalf_ReturnsLogHalftimeCriteria()
    {
        // Arrange
        var match = new Match
        {
            TypeId = SettleType.FirstHalf
        };

        // Act
        var criteria = service.ShowCreateCriteria(match, matchChecking);

        // Assert
        Assert.IsType<ShowLogHalftimeCriteria>(criteria);
    }

    [Fact]
    public void CreateCriteria_MatchIsSecondHalf_ReturnsLogFullTimeCriteria()
    {
        // Arrange
        var match = new Match
        {
            TypeId = SettleType.SecondHalf
        };

        // Act
        var criteria = service.ShowCreateCriteria(match, matchChecking);

        // Assert
        Assert.IsType<ShowLogFullTimeCriteria>(criteria);
    }
}