using AutoInstant.Services;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoInstant.Models;
using Xunit;

namespace AutoInstant.UnitTests.Services
{
    public class AutoInstantControllerTests
    {
        private readonly MatchSettlingPoolService matchSettlingPoolService;

        private readonly MatchSettleController matchSettleController;

        private readonly AutoInstantController controller;

        public AutoInstantControllerTests()
        {
            matchSettlingPoolService = Substitute.For<MatchSettlingPoolService>();
            matchSettleController = Substitute.For<MatchSettleController>();
            controller = new AutoInstantControllerImp(matchSettlingPoolService, matchSettleController);
        }

        [Fact]
        public async Task ProcessAsync_HasMatches_InvokeProcessMethod()
        {
            // Arrange
            var match = new Match { MatchId = 123, IsChecking = false };
            var matches = new List<Match> { match };

            matchSettlingPoolService.PeekNewMatch().Returns(matches);

            // Act
            await controller.ProcessAsync();

            // Assert
            await matchSettleController.Received().Process(match);
        }
    }
}