using AutoInstant.Exceptions;
using AutoInstant.Models;
using AutoInstant.Services;
using NSubstitute;
using Xunit;

namespace AutoInstant.UnitTests.Services
{
    
    public class ValidationServiceTests
    {
        [Fact]
        public void VerifyMatchBeforeExecution_NotThrowException()
        {
            // Arrange
            var validationScoreChangeService = Substitute.For<ValidationScoreChangeService>();
            var validationSettingMatchService = Substitute.For<ValidationSettingMatchService>();
            var validationStatusMatchService = Substitute.For<ValidationStatusMatchService>();
            var service = new ValidationServiceImp(validationScoreChangeService, validationSettingMatchService, validationStatusMatchService);

            // Assert
            var exception = Record.Exception(() => service.VerifyMatchBeforeExecution(new Match(), new MatchChecking()));
            Assert.Null(exception);
        }

        [Fact]
        public void VerifyMatchBeforeExecution_InvokeRightOrderService()
        {
            // Arrange
            var validationScoreChangeService = Substitute.For<ValidationScoreChangeService>();
            var validationSettingMatchService = Substitute.For<ValidationSettingMatchService>();
            var validationStatusMatchService = Substitute.For<ValidationStatusMatchService>();
            var service = new ValidationServiceImp(validationScoreChangeService, validationSettingMatchService, validationStatusMatchService);

            // Act
            service.VerifyMatchBeforeExecution(new Match(), new MatchChecking());

            // Assert
            Received.InOrder(() =>
            {
                validationSettingMatchService.Received().VerifySettingMatch(Arg.Any<MatchChecking>());
                validationStatusMatchService.Received().VerifyStatusMatch(Arg.Any<Match>(), Arg.Any<MatchChecking>());
                validationScoreChangeService.Received().VerifyScoreChanged(Arg.Any<Match>(), Arg.Any<MatchChecking>());
            });
        }

        [Fact]
        public void VerifyMatchAfterComparing_ThrowScoreMismatchedException()
        {
            // Arrange
            var validationScoreChangeService = Substitute.For<ValidationScoreChangeService>();
            var validationSettingMatchService = Substitute.For<ValidationSettingMatchService>();
            var validationStatusMatchService = Substitute.For<ValidationStatusMatchService>();
            var service = new ValidationServiceImp(validationScoreChangeService, validationSettingMatchService, validationStatusMatchService);

            // Assert
            Assert.Throws<ScoreMismatchedException>(() => service.VerifyMatchAfterComparing(new Match(), new MatchChecking()));
        }
    }
}