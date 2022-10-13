using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using AutoInstant.Services;

namespace AutoInstant.UnitTests.Services;

public class PushingCalculationCriteriaServiceTests
{
    private readonly PushingCalculationCriteriaService service;

    public PushingCalculationCriteriaServiceTests()
    {
        service = new PushingCalculationCriteriaServiceImp();
    }

    [Fact]
    public void CreateCriteria_MatchIsFirstHalf_ReturnsPushingCalculationHalftimeCriteria()
    {
        // Arrange
        var match = new Match
        {
            TypeId = SettleType.FirstHalf
        };

        // Act
        var criteria = service.CreateCriteria(match, string.Empty);

        // Assert
        Assert.IsType<PushingCalculationHalftimeCriteria>(criteria);
    }

    [Fact]
    public void CreateCriteria_MatchIsSecondHalf_ReturnsPushingCalculationFullTimeCriteria()
    {
        // Arrange
        var match = new Match
        {
            TypeId = SettleType.SecondHalf
        };

        // Act
        var criteria = service.CreateCriteria(match, string.Empty);

        // Assert
        Assert.IsType<PushingCalculationFullTimeCriteria>(criteria);
    }
}
