using AutoInstant.Basketball.Criteria;
using AutoInstant.Basketball.Services;
using Fanex.Data.Repository;

namespace AutoInstant.Basketball.UnitTests.Services;

public class LogServiceTests
{
    private readonly IDynamicRepository repository;

    private readonly LogService service;

    public LogServiceTests()
    {
        repository = Substitute.For<IDynamicRepository>();
        service = new LogServiceImp(repository);
    }

    [Fact]
    public void WriteDirectlyLog_ShouldExecute_WithCorrectCriteria()
    {
        // Arrange
        var settleCriteria = new SettleProfileCriteria
        {
            MatchId = 1,
            SelectedResources = "Resource1,Resource2",
            Action = "CAl"
        };

        // Act
        service.WriteDirectlyLog(settleCriteria);

        // Assert
        repository.Received().Execute(Arg.Any<LogDirectlyCriteria>());
    }

    [Fact]
    public void WriteLog_Always_ExecuteCriteria()
    {
        // Act
        service.WriteLog(1, "");

        // Assert
        repository.Received().Execute(Arg.Any<LogCriteria>());
    }
}