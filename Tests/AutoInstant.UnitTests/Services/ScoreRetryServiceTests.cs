using System.Threading.Tasks;
using AutoInstant.Models;
using AutoInstant.Services;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace AutoInstant.UnitTests.Services
{
    public class ScoreRetryServiceTests
    {
        private readonly ScoreMatcherService scoreMatcherService;
        private readonly ScoreRetryService scoreRetryService;

        public ScoreRetryServiceTests()
        {
            var settings = new AppSettingsKey()
            {
                RetryInterval = 3,
                RetryCount = 2,
                EnvironmentName = "Local"
            };
            var appSettingsKey = Options.Create(settings);

            scoreMatcherService = Substitute.For<ScoreMatcherService>();
            scoreRetryService = new ScoreRetryServiceImp(scoreMatcherService, appSettingsKey);
        }

        [Fact]
        public async Task IsScoreMatch_NoScoreMatched_ReturnsFalse()
        {
            // Arrange
            var match = new Match();
            scoreMatcherService.IsScoreMatch(Arg.Any<Match>()).Returns(false);

            // Act
            var result = await scoreRetryService.IsScoreMatch(match);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsScoreMatch_HasScoreMatched_ReturnsTrue()
        {
            // Arrange
            var match = new Match();
            scoreMatcherService.IsScoreMatch(Arg.Any<Match>()).Returns(true);

            // Act
            var result = await scoreRetryService.IsScoreMatch(match);

            // Assert
            Assert.True(result);
        }
    }
}