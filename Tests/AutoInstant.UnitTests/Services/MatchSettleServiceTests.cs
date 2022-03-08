using AutoInstant.Exceptions;
using AutoInstant.Models;
using AutoInstant.Services;
using NSubstitute;
using Xunit;

namespace AutoInstant.UnitTests.Services
{
    public class MatchSettleServiceTests
    {
        private readonly ValidationService validationService;
        private readonly PushingCalculationService pushingCalculationService;
        private readonly LogService logService;
        private readonly AutoInstantResultService autoInstantResultService;
        private readonly MatchSettleService matchSettleService;
        private readonly NotificationService notificationService;
        private readonly Match match;
        private readonly MatchChecking matchChecking;

        public MatchSettleServiceTests()
        {
            match = new Match();
            matchChecking = new MatchChecking();
            validationService = Substitute.For<ValidationService>();
            pushingCalculationService = Substitute.For<PushingCalculationService>();
            logService = Substitute.For<LogService>();
            autoInstantResultService = Substitute.For<AutoInstantResultService>();
            notificationService = Substitute.For<NotificationService>();
            matchSettleService = new MatchSettleServiceImp(validationService, pushingCalculationService, autoInstantResultService, logService, notificationService);
        }

        [Fact]
        public void ScoreMatched_VerifyMatchFail_InvokeActionLogAndInsertInstantResult()
        {
            // Arrange
            var exception = new AutoInstantException
            {
                Description = "exception"
            };
            validationService.When(x => x.VerifyMatchBeforeExecution(Arg.Any<Match>(), Arg.Any<MatchChecking>())).Do(x => { throw exception; });

            // Act
            matchSettleService.ScoreMatched(match, matchChecking);

            // Assert
            logService.Received().ActionLog(Arg.Is<Match>(m => m == match), Arg.Any<MatchChecking>(), exception);
            autoInstantResultService.Received().Insert(match, matchChecking);
            notificationService.Received().AddNotification(Arg.Any<Match>(), Arg.Any<bool>(), Arg.Any<string>());
        }

        [Fact]
        public void ScoreMatched_VerifyMatchSuccess_PushMatchIntoQueueSuccessThenNotCallActionLog()
        {
            // Arrange
            var result = new Models.Result();
            pushingCalculationService.Push(Arg.Any<Match>(), Arg.Any<MatchChecking>()).Returns(result);

            // Act
            matchSettleService.ScoreMatched(match, matchChecking);

            // Assert
            pushingCalculationService.Received().Push(Arg.Is<Match>(m => m == match), Arg.Is<MatchChecking>(mc => mc == matchChecking));
            logService.DidNotReceive().ActionLog(Arg.Any<Match>(), Arg.Any<MatchChecking>(), Arg.Any<string>());
            notificationService.DidNotReceive().AddNotification(Arg.Any<Match>(), Arg.Any<bool>(), Arg.Any<string>());
        }

        [Fact]
        public void ScoreMatched_VerifyMatchSuccess_PushMatchIntoQueueFailThenCallActionLog()
        {
            // Arrange
            var result = new Models.Result
            {
                ErrorMessage = "error"
            };
            pushingCalculationService.Push(Arg.Any<Match>(), Arg.Any<MatchChecking>()).Returns(result);

            // Act
            matchSettleService.ScoreMatched(match, matchChecking);

            // Assert
            pushingCalculationService.Received().Push(Arg.Is<Match>(m => m == match), Arg.Is<MatchChecking>(mc => mc == matchChecking));
            logService.Received().ActionLog(Arg.Any<Match>(), Arg.Any<MatchChecking>(), Arg.Is<string>(x => x == "error"));
            notificationService.DidNotReceive().AddNotification(Arg.Any<Match>(), Arg.Any<bool>(), Arg.Any<string>());
        }

        [Fact]
        public void ScoreMatched_VerifyMatchFail_InvokeThreeFunctions()
        {
            // Arrange
            var exception = new AutoInstantException
            {
                Description = "exception"
            };
            validationService.When(x => x.VerifyMatchBeforeExecution(Arg.Any<Match>(), Arg.Any<MatchChecking>())).Do(x => { throw exception; });

            // Act
            matchSettleService.ScoreMatched(match, matchChecking);

            // Assert
            logService.Received().ActionLog(Arg.Any<Match>(), Arg.Any<MatchChecking>(), exception);
            autoInstantResultService.Received().Insert(Arg.Any<Match>(), Arg.Any<MatchChecking>());
            notificationService.Received().AddNotification(Arg.Any<Match>(), Arg.Any<bool>(), Arg.Any<string>());
        }


        [Fact]
        public void ScoreMismatched_VerifyMatchFail_InvokeActionLogAndInsertInstantResult()
        {
            // Arrange
            var exception = new AutoInstantException
            {
                Description = "exception"
            };
            validationService.When(x => x.VerifyMatchAfterComparing(Arg.Any<Match>(), Arg.Any<MatchChecking>())).Do(x => { throw exception; });

            // Act
            matchSettleService.ScoreMismatched(match, matchChecking);

            // Assert
            logService.Received().ActionLog(Arg.Is<Match>(m => m == match), Arg.Any<MatchChecking>(), exception);
            autoInstantResultService.Received().Insert(match, matchChecking);
            notificationService.Received().AddNotification(Arg.Any<Match>(), Arg.Any<bool>(), Arg.Any<string>());
        }

        [Fact]
        public void ScoreMismatched_VerifyMatchSuccess_NotCallAnyMethod()
        {
            // Act
            matchSettleService.ScoreMismatched(match, matchChecking);

            // Assert
            logService.DidNotReceive().ActionLog(Arg.Any<Match>(), Arg.Any<MatchChecking>(), Arg.Any<string>());
            autoInstantResultService.DidNotReceive().Insert(match, matchChecking);
            notificationService.DidNotReceive().AddNotification(Arg.Any<Match>(), Arg.Any<bool>(), Arg.Any<string>());
        }
    }
}