using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using AutoInstant.Services;
using Fanex.Data.Repository;

namespace AutoInstant.UnitTests.Services;

public class AutoInstantResultServiceTests
{
    private IDynamicRepository repository;
    private AutoInstantResultService autoInstantResultService;

    public AutoInstantResultServiceTests()
    {
        repository = Substitute.For<IDynamicRepository>();
        autoInstantResultService = new AutoInstantResultServiceImp(repository);
    }

    [Fact]
    public async Task Insert_MatchIsNotFullTimeCalculated_InvokeExecuteRepository()
    {
        // Arrange
        repository = Substitute.For<IDynamicRepository>();
        autoInstantResultService = new AutoInstantResultServiceImp(repository);

        // Act
        await autoInstantResultService.Insert(new Match { TypeId = SettleType.SecondHalf }, new MatchChecking { IsCalculatedFT = false });

        // Assert
        await repository.Received(1).ExecuteAsync(Arg.Any<NonQueryCommand>());
    }

    [Fact]
    public async Task Insert_MatchIsFullTimeCalculated_NotInvokeExecuteRepository()
    {
        // Act
        await autoInstantResultService.Insert(new Match { TypeId = SettleType.SecondHalf }, new MatchChecking { IsCalculatedFT = true });

        // Assert
        await repository.Received(0).ExecuteAsync(Arg.Any<NonQueryCommand>());
    }

    [Fact]
    public async Task Insert_MatchIsNotIsManualInstCalculated_InvokeCriteriaWithIsInstCalculatedFalse()
    {
        // Arrange
        repository = Substitute.For<IDynamicRepository>();
        autoInstantResultService = new AutoInstantResultServiceImp(repository);

        // Act
        await autoInstantResultService.Insert(new Match { HomeValue = 1, AwayValue = 2 }, new MatchChecking { IsInstCalculated = true, PreviousHomeScore = "1", PreviousAwayScore = "0" });

        // Assert
        await repository.Received(1).ExecuteAsync(Arg.Is<AddAutoResultCriteria>(criteria => !criteria.IsInstCalculated));
    }

    [Fact]
    public async Task Insert_MatchIsIsManualInstCalculated_InvokeCriteriaWithIsInstCalculatedTrue()
    {
        // Arrange
        repository = Substitute.For<IDynamicRepository>();
        autoInstantResultService = new AutoInstantResultServiceImp(repository);

        // Act
        await autoInstantResultService.Insert(new Match { HomeValue = 1, AwayValue = 2 }, new MatchChecking { IsInstCalculated = true, PreviousHomeScore = "1", PreviousAwayScore = "2" });

        // Assert
        await repository.Received(1).ExecuteAsync(Arg.Is<AddAutoResultCriteria>(criteria => criteria.IsInstCalculated));
    }
}