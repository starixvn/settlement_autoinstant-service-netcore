using AutoInstant.Models;
using AutoInstant.Services;

namespace AutoInstant.UnitTests.Services;

public class MatchSettleControllerTests
{
    private readonly MatchSettleService matchSettleService;
    private readonly MatchSettlingPoolService matchSettlingPoolService;
    private readonly MatchSettleController matchSettleController;
    private readonly MatchInfo matchChecking = new();

    public MatchSettleControllerTests()
    {
        matchSettleService = Substitute.For<MatchSettleService>();
        matchSettlingPoolService = Substitute.For<MatchSettlingPoolService>();
        var matchQueryService = Substitute.For<MatchQueryService>();
        matchQueryService.GetMatchInfo(Arg.Any<Match>()).Returns(matchChecking);

        matchSettleController = new MatchSettleControllerImp(
            matchSettleService,
            matchSettlingPoolService,
            matchQueryService);
    }

    [Fact]
    public void Process_ScoreIsMatched_InvokeRightMethods()
    {
        // Arrange
        var match = new Match
        {
            IsChecking = false
        };

        // Act
        matchSettleController.Process(match);

        // Assert
        Assert.True(match.IsChecking);
        matchSettleService.Received().ScoreMatched(Arg.Is<Match>(m => m == match), matchChecking);
        matchSettlingPoolService.Received().Remove(match);
    }

    [Fact]
    public void Process_IsCornerMatch_InvokeRightMethods()
    {
        // Arrange
        var match = new Match
        {
            IsChecking = false,
            ChildMatchType = 204
        };

        // Act
        matchSettleController.Process(match);

        // Assert
        Assert.True(match.IsChecking);
        matchSettleService.Received().ScoreMatched(Arg.Is<Match>(m => m == match), matchChecking);
        matchSettlingPoolService.Received().Remove(match);
    }
}