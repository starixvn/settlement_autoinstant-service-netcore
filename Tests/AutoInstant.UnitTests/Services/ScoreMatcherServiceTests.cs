using AutoInstant.Models;
using AutoInstant.Services;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AutoInstant.UnitTests.Services
{
    public class ScoreMatcherServiceTests
    {
        private readonly ScoreService scoreService;
        private readonly ScoreMatcherService scoreMatcherService;

        public ScoreMatcherServiceTests()
        {
            scoreService = Substitute.For<ScoreService>();
            scoreMatcherService = new ScoreMatcherServiceImp(scoreService);
        }

        [Fact]
        public async Task IsScoreMatch_NoScoreMatched_ReturnsFalse()
        {
            // Arrange
            var match = new Match
            {
                HomeValue = 1,
                AwayValue = 0,
                TypeId = 1
            };
            var scores = new List<Score>
            {
                new Score
                {
                    MatchId = 1,
                    HomeValue = "1",
                    AwayValue = "2",
                    TypeId = 1
                }
            };
            scoreService.GetScore(Arg.Any<int>()).Returns(scores);

            // Act
            var isMatched = await scoreMatcherService.IsScoreMatch(match);

            // Assert
            Assert.False(isMatched);
        }

        [Fact]
        public async Task IsScoreMatch_HasScoreMatched_ReturnsTrue()
        {
            // Arrange
            var match = new Match
            {
                HomeValue = 1,
                AwayValue = 0,
                TypeId = 1
            };
            var scores = new List<Score>
            {
                new Score
                {
                    HomeValue = "1",
                    AwayValue = "0",
                    TypeId = 1
                }
            };
            scoreService.GetScore(Arg.Any<int>()).Returns(scores);

            // Act
            var isMatched = await scoreMatcherService.IsScoreMatch(match);

            // Assert
            Assert.True(isMatched);
        }
    }
}