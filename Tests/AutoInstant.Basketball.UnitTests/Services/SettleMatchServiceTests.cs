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
    public void Process_Always_ValidateAndHandleResultInvalidValid()
    {
        var resultDetails = new List<ResultDetail>
        {
            new()
            {
                ResourceId = "Q1",
            },
            new()
            {
                ResourceId = "Q2",
            }
        };

        scoreValidateService.IsScoreDecreased(Arg.Do<Match>(m => m.ResultDetails.First().Invalid = true));

        service.Process(new Match { MatchId = 123, ResultDetails = resultDetails });

        matchSettleService.Received().Settle(123,
            Arg.Is<List<ResultDetail>>(r => r.Count == 1 && r.First().ResourceId == "Q2"));

        matchSettleService.Received().HandleScoreValidateFailed(123,
            Arg.Is<List<ResultDetail>>(r => r.Count == 1 && r.First().ResourceId == "Q1"));

        matchSettlingPoolService.Received().Remove(Arg.Is<Match>(m => m.MatchId == 123));
    }
}