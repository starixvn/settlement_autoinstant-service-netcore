using AutoInstant.Result.Models;
using AutoInstant.Result.Services;

namespace AutoInstant.Result.UnitTests.Services;

public class ResourceServiceTests
{
    private readonly ActionResourceService actionResourceService;

    private readonly GoalRecordService goalRecordService;

    public ResourceServiceTests()
    {
        actionResourceService = Substitute.For<ActionResourceService>();
        goalRecordService = Substitute.For<GoalRecordService>();
    }

    [Fact]
    public void Halftime_HtFgNgAndFgNgIsNotExisted_ReturnsEmptyResources()
    {
        // Arrange
        var goal = new Goal();
        var service = new ResourceServiceImp(actionResourceService, goalRecordService);

        // Act
        var actual = service.Halftime(goal);

        // Assert
        Assert.Empty(actual);
    }

    [Fact]
    public void Halftime_IsExistedHtFgNg_ReturnsOneItemOnList()
    {
        // Arrange
        var goal = new Goal
        {
            IsExistHtFgNg = true,
            IsExistFgNg = false
        };

        var xmlResource = new XmlResource();
        actionResourceService.HalftimeFirstGoalResources(Arg.Any<string>()).Returns(xmlResource);
        var service = new ResourceServiceImp(actionResourceService, goalRecordService);

        // Act
        var actual = service.Halftime(goal);

        // Assert
        Assert.Single(actual);
    }

    [Fact]
    public void Halftime_IsExistedFgNg_ReturnsOneItemOnList()
    {
        // Arrange
        var goal = new Goal
        {
            IsExistHtFgNg = false,
            IsExistFgNg = true
        };

        var xmlResource = new XmlResource();
        actionResourceService.HalftimeFirstGoalResources(Arg.Any<string>()).Returns(xmlResource);
        actionResourceService.FullTimeFirstGoalResources(Arg.Any<string>(), Arg.Any<string>()).Returns(xmlResource);
        var service = new ResourceServiceImp(actionResourceService, goalRecordService);

        // Act
        var actual = service.Halftime(goal);

        // Assert
        Assert.Single(actual);
    }

    [Fact]
    public void Halftime_IsExistedHtFgNgAndIsExistedFgNg_ReturnsTwoItemsOnList()
    {
        // Arrange
        var goal = new Goal
        {
            IsExistHtFgNg = true,
            IsExistFgNg = true
        };

        var xmlResource = new XmlResource();
        actionResourceService.FullTimeFirstGoalResources(Arg.Any<string>(), Arg.Any<string>()).Returns(xmlResource);
        var service = new ResourceServiceImp(actionResourceService, goalRecordService);

        // Act
        var actual = service.Halftime(goal);

        // Assert
        Assert.Equal(2, actual.Count);
    }

    [Fact]
    public void FullTime_IsNotExistedFgNg_ReturnsEmptyResources()
    {
        // Arrange
        var goal = new Goal();
        var service = new ResourceServiceImp(actionResourceService, goalRecordService);

        // Act
        var actual = service.FullTime(goal);

        // Assert
        Assert.Empty(actual);
    }

    [Fact]
    public void FullTime_IsExistedFgNg_ReturnsOneItemOnList()
    {
        // Arrange
        var goal = new Goal
        {
            IsExistHtFgNg = false,
            IsExistFgNg = true
        };

        var xmlResource = new XmlResource();
        actionResourceService.FullTimeFirstGoalResources(Arg.Any<string>(), Arg.Any<string>()).Returns(xmlResource);
        var service = new ResourceServiceImp(actionResourceService, goalRecordService);

        // Act
        var actual = service.FullTime(goal);

        // Assert
        Assert.Single(actual);
    }
}