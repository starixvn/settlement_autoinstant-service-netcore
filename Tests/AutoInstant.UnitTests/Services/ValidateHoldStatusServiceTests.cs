using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Exceptions;
using AutoInstant.Models;
using AutoInstant.Services;
using Fanex.Data.Repository;

namespace AutoInstant.UnitTests.Services;

public class ValidateHoldStatusServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly ValidateHoldStatusServiceImp validateHoldStatusService;

    public ValidateHoldStatusServiceTests()
    {
        repository = Substitute.For<IDynamicRepository>();
        validateHoldStatusService = new ValidateHoldStatusServiceImp(repository);
    }

    [Fact]
    public async Task ValidateHoldStatus_ShouldThrowResultOnHoldException_WhenResultIsOnHold()
    {
        // Arrange
        var match = new Match { MatchId = 1, TypeId = SettleType.FirstHalf };
        var resultDetails = new List<ResultDetail>
        {
            new() { SourceId = 0, ResourceId = "01", ResultSetting = 1}
        };
        repository.FetchAsync<ResultDetail>(Arg.Any<GetResultDetailCriteria>())
                  .Returns(resultDetails);

        // Act & Assert
        await Assert.ThrowsAsync<ResultOnHoldException>(() => validateHoldStatusService.ValidateHoldStatus(match));
    }

    [Fact]
    public async Task ValidateHoldStatus_ShouldNotThrowException_WhenResultIsNotOnHold()
    {
        // Arrange
        var match = new Match { MatchId = 1, TypeId = SettleType.FirstHalf };
        var resultDetails = new List<ResultDetail>
        {
            new() { SourceId = 0, ResourceId = "01", ResultSetting = 0}
        };
        repository.FetchAsync<ResultDetail>(Arg.Any<GetResultDetailCriteria>())
                  .Returns(resultDetails);

        // Act & Assert
        await validateHoldStatusService.ValidateHoldStatus(match);
    }

    [Fact]
    public async Task ValidateHoldStatus_MatchFTHold_ThrowException()
    {
        // Arrange
        var match = new Match { MatchId = 1, TypeId = SettleType.SecondHalf };
        var resultDetails = new List<ResultDetail>
        {
            new() { SourceId = 0, ResourceId = "00", ResultSetting = 1 }
        };
        repository.FetchAsync<ResultDetail>(Arg.Any<GetResultDetailCriteria>())
                  .Returns(resultDetails);

        // Act & Assert
        await Assert.ThrowsAsync<ResultOnHoldException>(() => validateHoldStatusService.ValidateHoldStatus(match));
    }

    [Fact]
    public async Task ValidateHoldStatus_MatchFTHoldOnHT_ThrowException()
    {
        // Arrange
        var match = new Match { MatchId = 1, TypeId = SettleType.SecondHalf };
        var resultDetails = new List<ResultDetail>
        {
            new() { SourceId = 0, ResourceId = "01", ResultSetting = 1 },
            new() { SourceId = 0, ResourceId = "00", ResultSetting = 0 }
        };
        repository.FetchAsync<ResultDetail>(Arg.Any<GetResultDetailCriteria>())
            .Returns(resultDetails);

        // Act & Assert
        await Assert.ThrowsAsync<ResultOnHoldException>(() => validateHoldStatusService.ValidateHoldStatus(match));
    }
}