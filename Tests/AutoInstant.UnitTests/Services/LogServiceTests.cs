using System.Threading.Tasks;
using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Exceptions;
using AutoInstant.Models;
using AutoInstant.Services;
using Fanex.Data.Repository;
using NSubstitute;
using Xunit;

namespace AutoInstant.UnitTests.Services
{
    public class LogServiceTests
    {
        private readonly IDynamicRepository repository;
        private readonly LogService logService;
        private readonly MatchChecking matchChecking;

        public LogServiceTests()
        {
            matchChecking = new MatchChecking();
            repository = Substitute.For<IDynamicRepository>();
            var logCriteriaService = new LogCriteriaServiceImp();
            logService = new LogServiceImp(repository, logCriteriaService);
        }

        [Fact]
        public async Task ActionLog_MatchIsHalfTimeAndAlwaysHideReferenceActions_InvokeCriteriaWithActionId197()
        {
            // Arrange
            var match = new Match
            {
                TypeId = SettleType.FirstHalf
            };

            // Act
            await logService.ActionLog(match, matchChecking, "HaftTime");

            // Assert
            await  repository.Received().ExecuteAsync(Arg.Is<LogCriteria>(criteria =>
                criteria.ActionId == 197 && criteria.Remark == "HaftTime"
            ));
        }

        [Fact]
        public async Task ActionLog_MatchIsFullTimeAndAlwaysHideReferenceActions_InvokeCriteriaWithActionId198()
        {
            // Arrange
            var match = new Match
            {
                TypeId = SettleType.SecondHalf
            };

            // Act
            await logService.ActionLog(match, matchChecking, "SecondHalf");

            // Assert
            await repository.Received().ExecuteAsync(Arg.Is<LogCriteria>(criteria =>
                criteria.ActionId == 198 && criteria.Remark == "SecondHalf"
            ));
        }

        [Fact]
        public async Task ActionLog_MatchIsHalfTimeAndShowReferenceActions_InvokeCriteriaWithActionId195()
        {
            // Arrange
            var match = new Match
            {
                TypeId = SettleType.FirstHalf
            };

            var exception = new AutoInstantException { IsShowOnReferenceActions = true, Description = "HaftTime" };

            // Act
            await logService.ActionLog(match, matchChecking, exception);

            // Assert
            await repository.Received().ExecuteAsync(Arg.Is<LogCriteria>(criteria =>
                criteria.ActionId == 195 && criteria.Remark == "HaftTime"
            ));
        }

        [Fact]
        public async Task ActionLog_MatchIsFullTimeAndShowReferenceActions_InvokeCriteriaWithActionId196()
        {
            // Arrange
            var match = new Match
            {
                TypeId = SettleType.SecondHalf
            };

            var exception = new AutoInstantException { IsShowOnReferenceActions = true, Description = "SecondHalf" };

            // Act
            await logService.ActionLog(match, matchChecking, exception);

            // Assert
            await repository.Received().ExecuteAsync(Arg.Is<LogCriteria>(criteria =>
                criteria.ActionId == 196 && criteria.Remark == "SecondHalf"
            ));
        }

        [Fact]
        public async Task ActionLog_MatchIsHalfTimeAndHideReferenceActions_InvokeCriteriaWithActionId197()
        {
            // Arrange
            var match = new Match
            {
                TypeId = SettleType.FirstHalf
            };

            var exception = new AutoInstantException { IsShowOnReferenceActions = false, Description = "HaftTime" };

            // Act
            await logService.ActionLog(match, matchChecking, exception);

            // Assert
            await repository.Received().ExecuteAsync(Arg.Is<LogCriteria>(criteria =>
                criteria.ActionId == 197 && criteria.Remark == "HaftTime"
            ));
        }

        [Fact]
        public async Task ActionLog_MatchIsFullTimeAndHideReferenceActions_InvokeCriteriaWithActionId198()
        {
            // Arrange
            var match = new Match
            {
                TypeId = SettleType.SecondHalf
            };

            var exception = new AutoInstantException { IsShowOnReferenceActions = false, Description = "SecondHalf" };

            // Act
            await logService.ActionLog(match, matchChecking, exception);

            // Assert
            await repository.Received().ExecuteAsync(Arg.Is<LogCriteria>(criteria =>
                criteria.ActionId == 198 && criteria.Remark == "SecondHalf"
            ));
        }
    }
}