using AutoInstant.Result.Services;
using Xunit;

namespace AutoInstant.Result.UnitTests.Services
{
    public class GoalRecordServiceTests
    {
        private readonly GoalRecordService service;

        public GoalRecordServiceTests()
        {
            service = new GoalRecordServiceImp();
        }

        [Theory]
        [InlineData(";AHA", "")]
        [InlineData("", "")]
        [InlineData(";", "")]
        [InlineData("AHAAH;AHA", "AHAAH")]
        [InlineData("HAA", "HAA")]
        public void HalftimeGoal_Always_ReturnsExpectedGoal(string goalRecord, string expectedGoal)
        {
            // Act
            var actual = service.HalftimeGoal(goalRecord);

            // Assert
            Assert.Equal(expectedGoal, actual);
        }

        [Theory]
        [InlineData("AH;", "")]
        [InlineData("HAAHA", "")]
        [InlineData("", "")]
        [InlineData(";", "")]
        [InlineData(";AHA", "AHA")]
        [InlineData("AH;AHAH", "AHAH")]
        public void FullTimeGoal_Always_ReturnsExpectedGoal(string goalRecord, string expectedGoal)
        {
            // Act
            var actual = service.FullTimeGoal(goalRecord);

            // Assert
            Assert.Equal(expectedGoal, actual);
        }
    }
}