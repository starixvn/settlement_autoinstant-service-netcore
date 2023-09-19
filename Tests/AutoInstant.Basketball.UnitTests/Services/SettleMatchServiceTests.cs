using AutoInstant.Basketball.Models;
using AutoInstant.Basketball.Services;

namespace AutoInstant.Basketball.UnitTests.Services;

public class SettleMatchServiceTests
{
    private readonly MatchSettlingPoolService matchSettlingPoolService;

    private readonly ScoreValidateService scoreValidateService;

    private readonly MatchSettleService matchSettleService;

    private readonly SettleMatchService service;

    public SettleMatchServiceTests()
    {
        matchSettleService = Substitute.For<MatchSettleService>();
        matchSettlingPoolService = Substitute.For<MatchSettlingPoolService>();
        scoreValidateService = Substitute.For<ScoreValidateService>();
        service = new SettleMatchServiceImp(matchSettlingPoolService, scoreValidateService, matchSettleService);
    }

    [Fact]
    public void Process_Always_InvokeRightOrderMethod()
    {
        var match = new Match();
        service.Process(match);

        Received.InOrder(() =>
        {
            scoreValidateService.ScoreValidation(match);

            matchSettleService.Settle(Arg.Any<Match>());

            matchSettleService.HandleScoreFailed(Arg.Any<Match>());

            matchSettlingPoolService.Remove(Arg.Any<Match>());
        });
    }
}