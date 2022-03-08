using AutoInstant.Exceptions;
using AutoInstant.Models;
using AutoInstant.Services;
using Xunit;

namespace AutoInstant.UnitTests.Services
{

    public class ValidationScoreChangeServiceTests
    {
        private readonly ValidationScoreChangeService service;

        public ValidationScoreChangeServiceTests()
        {
            service = new ValidationScoreChangeServiceImp();
        }

        [Fact]
        public void VerifyScoreChanged_IsWrongScoreIsFalse_NotThrowException()
        {
            // Arrange
            var match = new Match
            {
                MatchId = 1,
                HomeValue = 1,
                AwayValue = 2,
                InstantKey = 1
            };

            var matchChecking = new MatchChecking
            {
                PreviousHomeScore = "1",
                PreviousAwayScore = "1",
                InstantKey = 2
            };

            // Assert
            var exception = Record.Exception(() => service.VerifyScoreChanged(match, matchChecking));
            Assert.Null(exception);
        }

        [Fact]
        public void VerifyScoreChanged_IsWrongScoreAndInstantKeyOfMatchLessThanMatchChecking_ThrowManualCalculatedException()
        {
            // Arrange
            var match = new Match
            {
                MatchId = 1,
                HomeValue = 1,
                AwayValue = 2,
                InstantKey = 1
            };

            var matchChecking = new MatchChecking
            {
                PreviousHomeScore = "2",
                PreviousAwayScore = "1",
                InstantKey = 2
            };

            // Assert
            Assert.Throws<ManualCalculatedException>(() => service.VerifyScoreChanged(match, matchChecking));
        }

        [Fact]
        public void VerifyScoreChanged_IsWrongScoreAndInstantKeyOfMatchGreaterThanMatchChecking_ThrowScoreChangedException()
        {
            // Arrange
            var match = new Match
            {
                MatchId = 1,
                HomeValue = 1,
                AwayValue = 2,
                InstantKey = 2
            };

            var matchChecking = new MatchChecking
            {
                PreviousHomeScore = "2",
                PreviousAwayScore = "1",
                InstantKey = 1
            };

            // Assert
            Assert.Throws<ScoreChangedException>(() => service.VerifyScoreChanged(match, matchChecking));
        }
    }
}