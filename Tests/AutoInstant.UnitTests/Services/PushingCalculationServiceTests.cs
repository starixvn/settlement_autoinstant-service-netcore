using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using AutoInstant.Result.Models;
using AutoInstant.Result.Services;
using AutoInstant.Services;
using Fanex.Data.Repository;
using Fanex.Settlement.ScaleOut.Models;
using Fanex.Settlement.ScaleOut.Request;
using Fanex.Settlement.ScaleOut.Services;
using Microsoft.Extensions.Options;

namespace AutoInstant.UnitTests.Services;

public class PushingCalculationServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly ResultsService resultsService;
    private readonly PushingCalculationCriteriaService pushingCalculationCriteriaService;
    private IOptions<AppSettingsKey> appSettingsKey;
    private readonly ScaleOutSettleService scaleOutSettleService;
    private readonly AutoInstantResultService autoInstantResultService;
    private readonly LogDirectlyService logDirectlyService;

    public PushingCalculationServiceTests()
    {
        pushingCalculationCriteriaService = Substitute.For<PushingCalculationCriteriaService>();
        repository = Substitute.For<IDynamicRepository>();
        resultsService = Substitute.For<ResultsService>();
        scaleOutSettleService = Substitute.For<ScaleOutSettleService>();
        autoInstantResultService = Substitute.For<AutoInstantResultService>();
        logDirectlyService = Substitute.For<LogDirectlyService>();

        appSettingsKey = Options.Create(new AppSettingsKey { SiteDbServer = new() { "ABC" } });

        resultsService.HalftimeResults(Arg.Any<Goal>()).Returns("HalftimeResults");
        resultsService.FullTimeResults(Arg.Any<Goal>()).Returns("FullTimeResults");

        scaleOutSettleService.ValidateEventAndTicket(Arg.Any<EventTicketRequest>()).Returns(new ResultMessage { ErrorMessage = "" });
    }

    [Fact]
    public async Task Push_MatchIsFirstHalf_InvokeExecuteWithRightCriteria()
    {
        // Arrange
        var match = new Match
        {
            TypeId = SettleType.FirstHalf,
        };
        var matchChecking = new MatchInfo();

        pushingCalculationCriteriaService.CreateCriteria(Arg.Any<Match>(), "HalftimeResults")
            .Returns(new PushingCalculationCriteria(new Match()) { ResourceResult = "HalftimeResults" });

        var pushingCalculationService = new PushingCalculationServiceImp(
            repository,
            resultsService,
            pushingCalculationCriteriaService,
            scaleOutSettleService,
            autoInstantResultService,
            appSettingsKey,
            logDirectlyService
            );

        // Act
        var result = await pushingCalculationService.Push(match, matchChecking);

        // Assert
        repository.Received().Execute(Arg.Is<PushingCalculationCriteria>(criteria =>
           criteria.ResourceResult == "HalftimeResults"
       ));
        Assert.Equal(result.ErrorMessage, string.Empty);
    }

    [Fact]
    public async Task Push_MatchIsSecondHalf_InvokeExecuteWithRightCriteria()
    {
        // Arrange
        var match = new Match
        {
            TypeId = SettleType.SecondHalf
        };
        var matchChecking = new MatchInfo();
        pushingCalculationCriteriaService.CreateCriteria(Arg.Any<Match>(), Arg.Any<string>())
            .Returns(new PushingCalculationCriteria(new Match()) { ResourceResult = "FullTimeResults" });

        var pushingCalculationService = new PushingCalculationServiceImp(
            repository,
            resultsService,
            pushingCalculationCriteriaService,
            scaleOutSettleService,
            autoInstantResultService,
            appSettingsKey,
            logDirectlyService
            );

        // Act
        var result = await pushingCalculationService.Push(match, matchChecking);

        // Assert
        repository.Received().Execute(Arg.Is<PushingCalculationCriteria>(criteria =>
            criteria.ResourceResult == "FullTimeResults"
       ));
        Assert.Equal(result.ErrorMessage, string.Empty);
    }

    [Fact]
    public async Task Push_MatchIsFirstHalfAndExecuteFail_ReturnsFailureResult()
    {
        // Arrange
        var match = new Match
        {
            TypeId = SettleType.SecondHalf
        };
        var matchChecking = new MatchInfo();
        var fakeCriteria = new PushingCalculationHalftimeCriteria(match, "")
        {
            ErrCode = 1,
            ErrMsg = "Fail"
        };
        scaleOutSettleService.ValidateEventAndTicket(Arg.Any<EventTicketRequest>()).Returns(new ResultMessage { ErrorMessage = "Fail" });

        pushingCalculationCriteriaService.CreateCriteria(Arg.Any<Match>(), Arg.Any<string>()).Returns(fakeCriteria);

        var pushingCalculationService = new PushingCalculationServiceImp(
               repository,
               resultsService,
               pushingCalculationCriteriaService,
               scaleOutSettleService,
               autoInstantResultService,
               appSettingsKey,
               logDirectlyService
               );


        // Act
        var result = await pushingCalculationService.Push(match, matchChecking);

        // Assert
        Assert.True(result.ErrorMessage == "Fail");
    }
}