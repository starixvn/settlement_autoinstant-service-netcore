using AutoFixture;
using AutoInstant.Models;
using AutoInstant.Services;

namespace AutoInstant.UnitTests.Services;

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
    public void ProcessAsync_HasMatches_InvokeProcessMethod()
    {
        // Arrange
        var fixture = new Fixture();
        var matches = fixture.CreateMany<Match>(2).ToList();

        matchSettlingPoolService.PeekNewMatch().Returns(matches);

        // Act
        controller.ProcessAsync();

        // Assert
        matchSettleController.Received().Process(Arg.Any<Match>());
    }
}