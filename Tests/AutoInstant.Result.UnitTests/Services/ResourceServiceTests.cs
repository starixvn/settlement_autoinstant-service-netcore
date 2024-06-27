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
    public void Halftime_IsExistFirstCorner_ReturnsOneItemOnList()
    {
        // Arrange
        var goal = new Goal
        {
            IsExistFirstCorner = true
        };

        var xmlResource = new XmlResource();
        goalRecordService.FullTimeGoal(Arg.Any<string>()).Returns("goal");
        actionResourceService.FullTimeFirstCornerResource(Arg.Any<string>(), Arg.Any<string>()).Returns(xmlResource);
        var service = new ResourceServiceImp(actionResourceService, goalRecordService);

        // Act
        var actual = service.Halftime(goal);

        // Assert
        Assert.Single(actual);
    }

    [Fact]
    public void Halftime_IsExistHTCorner_ReturnsOneItemOnList()
    {
        // Arrange
        var goal = new Goal
        {
            IsExistHTCorner = true
        };

        var xmlResource = new XmlResource();
        actionResourceService.HalftimeExistCornerRaceResource(Arg.Any<string>()).Returns(new List<XmlResource>() { xmlResource });
        var service = new ResourceServiceImp(actionResourceService, goalRecordService);

        // Act
        var actual = service.Halftime(goal);

        // Assert
        Assert.Single(actual);
    }

    [Fact]
    public void Halftime_IsExistCorner_ReturnsOneItemOnList()
    {
        // Arrange
        var goal = new Goal
        {
            IsExistCorner = true
        };

        var xmlResource = new XmlResource();
        goalRecordService.FullTimeGoal(Arg.Any<string>()).Returns("goal");
        actionResourceService.FullTimeExistCornerRaceResource(Arg.Any<string>(), Arg.Any<string>()).Returns(new List<XmlResource>() { xmlResource });
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

    [Fact]
    public void FullTime_IsExistCorner_ReturnsOneItemCornerOnList()
    {
        // Arrange
        var goal = new Goal
        {
            IsExistCorner = true
        };

        var xmlResource = new XmlResource();
        actionResourceService.FullTimeExistCornerRaceResource(Arg.Any<string>(), Arg.Any<string>()).Returns(new List<XmlResource>() { xmlResource });
        var service = new ResourceServiceImp(actionResourceService, goalRecordService);

        // Act
        var actual = service.FullTime(goal);

        // Assert
        Assert.Single(actual);
    }

    [Fact]
    public void FullTime_IsExistFirstCorner_ReturnsOneItemFirstCornerOnList()
    {
        // Arrange
        var goal = new Goal
        {
            IsExistFirstCorner = true
        };

        var xmlResource = new XmlResource();
        actionResourceService.FullTimeFirstCornerResource(Arg.Any<string>(), Arg.Any<string>()).Returns(xmlResource);
        var service = new ResourceServiceImp(actionResourceService, goalRecordService);

        // Act
        var actual = service.FullTime(goal);

        // Assert
        Assert.Single(actual);
    }

    [Fact]
    public void FullTime_IsExist2HFirstCorner_ReturnsOneItem2HFirstCornerOnList()
    {
        // Arrange
        var goal = new Goal
        {
            IsExist2HFirstCorner = true
        };

        var xmlResource = new XmlResource();
        actionResourceService.FullTimeFirstCornerResource(Arg.Any<string>(), Arg.Any<string>()).Returns(xmlResource);
        goalRecordService.FullTimeGoal(Arg.Any<string>()).Returns("goal");
        var service = new ResourceServiceImp(actionResourceService, goalRecordService);

        // Act
        var actual = service.FullTime(goal);

        // Assert
        Assert.Single(actual);
    }


    [Fact]
    public void FullTime_IsExistAllBetType_ReturnsFullItemsOnList()
    {
        // Arrange
        var goal = new Goal
        {
            IsExist2HFirstCorner = true,
            IsExistFirstCorner = true,
            IsExistCorner = true,
            IsExistFgNg = true
        };

        var xmlResource = new XmlResource();
        actionResourceService.FullTimeExistCornerRaceResource(Arg.Any<string>(), Arg.Any<string>()).Returns(new List<XmlResource>() { xmlResource });
        actionResourceService.FullTimeExistCornerRaceResource(Arg.Any<string>(), Arg.Any<string>()).Returns(new List<XmlResource>() { xmlResource });
        actionResourceService.FullTimeFirstCornerResource(Arg.Any<string>(), Arg.Any<string>()).Returns(xmlResource);
        actionResourceService.FullTimeFirstCornerResource(Arg.Any<string>(), Arg.Any<string>()).Returns(xmlResource);
        goalRecordService.FullTimeGoal(Arg.Any<string>()).Returns("goal");
        var service = new ResourceServiceImp(actionResourceService, goalRecordService);

        // Act
        var actual = service.FullTime(goal);

        // Assert
        Assert.Equal(4, actual.Count);
    }
}