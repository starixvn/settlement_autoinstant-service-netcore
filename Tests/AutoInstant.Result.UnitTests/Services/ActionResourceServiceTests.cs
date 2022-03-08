using AutoInstant.Result.Models;
using AutoInstant.Result.Services;
using NSubstitute;
using Xunit;

namespace AutoInstant.Result.UnitTests.Services
{
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
    }
}