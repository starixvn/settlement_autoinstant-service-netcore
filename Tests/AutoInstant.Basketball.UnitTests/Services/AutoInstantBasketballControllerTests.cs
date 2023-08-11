using AutoFixture;
using AutoInstant.Basketball.Models;
using AutoInstant.Basketball.Services;

namespace AutoInstant.Basketball.UnitTests.Services;

public class AutoInstantBasketballControllerTests
{
    private readonly MatchSettlingPoolService matchSettlingPoolService;

    private readonly SettleMatchService settleMatchService;

    private readonly AutoInstantBasketballController controller;

    public AutoInstantBasketballControllerTests()
    {
        matchSettlingPoolService = Substitute.For<MatchSettlingPoolService>();
        settleMatchService = Substitute.For<SettleMatchService>();
        controller = new AutoInstantBasketballControllerImp(matchSettlingPoolService, settleMatchService);
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
        settleMatchService.Received(2).Process(Arg.Any<Match>());
    }
}