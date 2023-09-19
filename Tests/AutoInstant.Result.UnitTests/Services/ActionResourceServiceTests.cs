using AutoInstant.Result.Models;
using AutoInstant.Result.Services;

namespace AutoInstant.Result.UnitTests.Services;

public class ActionResourceServiceTests
{
    private readonly FirstGoalService firstGoalService;

    private static XmlResource CreateResource(string resourceId, string homeValue)
    {
        return new XmlResource
        {
            ResourceId = resourceId,
            ResultStatusId = 5,
            SettlementStatusId = 4,
            HomeValue = homeValue
        };
    }

    public ActionResourceServiceTests()
    {
        firstGoalService = Substitute.For<FirstGoalService>();
    }

    [Fact]
    public void HalftimeFirstGoalResources_Always_ReturnsExpectedResources()
    {
        // Arrange
        var goalRecord = Arg.Any<string>();
        firstGoalService.FirstGoal(goalRecord).Returns("A");
        var service = new ActionResourceServiceImp(firstGoalService);

        // Act
        var actualResources = service.HalftimeFirstGoalResources(goalRecord);

        // Assert
        var expectedResources = CreateResource("127F", "A");

        AssertExtension.AreEqual(expectedResources, actualResources);
    }

    [Fact]
    public void FullTimeFirstGoalResources_Always_ReturnsExpectedResources()
    {
        // Arrange
        var goalRecord = Arg.Any<string>();
        firstGoalService.FirstGoal(goalRecord).Returns("A");
        var service = new ActionResourceServiceImp(firstGoalService);

        // Act
        var actualResources = service.FullTimeFirstGoalResources(goalRecord, goalRecord);

        // Assert
        var expectedResources = CreateResource("14F", "A");

        AssertExtension.AreEqual(expectedResources, actualResources);
    }

    [Fact]
    public void HalfTimeFirstCornerResource_Always_ReturnsExpectedResources()
    {
        // Arrange
        var goalRecord = Arg.Any<string>();
        firstGoalService.FirstGoal(goalRecord).Returns("A");
        var service = new ActionResourceServiceImp(firstGoalService);

        // Act
        var actualResources = service.HalftimeFirstCornerResource(goalRecord);

        // Assert
        var expectedResources = CreateResource("489", "A");

        AssertExtension.AreEqual(expectedResources, actualResources);
    }

    [Fact]
    public void FullTimeFirstCornerResource_Always_ReturnsExpectedResources()
    {
        // Arrange
        var goalRecord = Arg.Any<string>();
        firstGoalService.FirstGoal(goalRecord).Returns("A");
        var service = new ActionResourceServiceImp(firstGoalService);

        // Act
        var actualResources = service.FullTimeFirstCornerResource(goalRecord, goalRecord);

        // Assert
        var expectedResources = CreateResource("488", "A");

        AssertExtension.AreEqual(expectedResources, actualResources);
    }

    [Fact]
    public void TwoHFirstCornerResource_Always_ReturnsExpectedResources()
    {
        // Arrange
        var goalRecord = Arg.Any<string>();
        firstGoalService.FirstGoal(goalRecord).Returns("A");
        var service = new ActionResourceServiceImp(firstGoalService);

        // Act
        var actualResources = service.TwoHFirstCornerResource(goalRecord);

        // Assert
        var expectedResources = CreateResource("490", "A");

        AssertExtension.AreEqual(expectedResources, actualResources);
    }

    [Fact]
    public void FullTimeExistCornerRaceResource_Always_ReturnsExpectedResources()
    {
        // Arrange
        var service = new ActionResourceServiceImp(firstGoalService);

        // Act
        var actualResources = service.FullTimeExistCornerRaceResource("A", "A");

        // Assert
        var expectedResources = new List<XmlResource> { CreateResource("48601", "A"), CreateResource("48602", "A") };

        Assert.Equal(expectedResources.Count, actualResources.Count);
    }

    [Fact]
    public void HalftimeExistCornerRaceResource_Always_ReturnsExpectedResources()
    {
        // Arrange
        var service = new ActionResourceServiceImp(firstGoalService);

        // Act
        var actualResources = service.HalftimeExistCornerRaceResource("AA");

        // Assert
        var expectedResources = new List<XmlResource> { CreateResource("48701", "A"), CreateResource("48702", "A") };

        Assert.Equal(expectedResources.Count, actualResources.Count);
    }
}