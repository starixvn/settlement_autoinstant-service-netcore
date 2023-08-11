using AutoInstant.Basketball.Constants;
using AutoInstant.Basketball.Criteria;
using AutoInstant.Basketball.Models;
using AutoInstant.Basketball.Services;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;

namespace AutoInstant.Basketball.UnitTests.Services;

public class MatchSettleServiceTests
{
    private readonly IDynamicRepository repository;

    private readonly LogService logService;

    private readonly GenerateService generateService;

    private readonly MatchSettleService service;

    public MatchSettleServiceTests()
    {
        repository = Substitute.For<IDynamicRepository>();
        logService = Substitute.For<LogService>();
        generateService = Substitute.For<GenerateService>();
        var settings = new AppSettingsBasketballKey()
        {
            SiteDbServer = new List<string> { "site1", "site2" }
        };
        var appSettingsOptions = Options.Create(settings);

        service = new MatchSettleServiceImp(repository, appSettingsOptions, logService, generateService);
    }

    [Fact]
    public void Settle_EmptyResultDetails_ShouldStopProgress()
    {
        service.Settle(123, new List<ResultDetail>());

        generateService.DidNotReceive().CreateResources(Arg.Any<List<ResultDetail>>());
    }

    [Fact]
    public void Settle_ExistResultDetails_UpdateAndSettleResult()
    {
        generateService.CreateResources(Arg.Is<List<ResultDetail>>(i => i.Count == 1 && i.First().ResourceId == "Q1"))
            .Returns("<Resources />");
        service.Settle(123, new List<ResultDetail> { new() { ResourceId = "Q1" } });

        generateService.Received().CreateResources(Arg.Is<List<ResultDetail>>(i => i.Count == 1 && i.First().ResourceId == "Q1"));
        repository.Received()
            .ExecuteAsync(Arg.Is<UpdateMatchCriteria>(c => c.MatchId == 123 && c.SelectedResources == "<Resources />" && c.StatusId == Status.Waitting));
        logService.Received().WriteDirectlyLog(Arg.Any<SettleProfileCriteria>());
        repository.Received(2).Execute(Arg.Any<SettleProfileCriteria>());
    }

    [Fact]
    public void HandleScoreValidateFailed_EmptyResultDetails_ShouldStopProgress()
    {
        service.HandleScoreValidateFailed(123, new List<ResultDetail>());

        generateService.DidNotReceive().CreateResources(Arg.Any<List<ResultDetail>>());
    }

    [Fact]
    public void HandleScoreValidateFailed_ExistResultDetails_UpdateAndLogResult()
    {
        generateService.CreateResources(Arg.Is<List<ResultDetail>>(i => i.Count == 1 && i.First().ResourceId == "Q1"))
            .Returns("<Resources />");
        service.HandleScoreValidateFailed(123, new List<ResultDetail> { new() { ResourceId = "Q1" } });

        generateService.Received().CreateResources(Arg.Is<List<ResultDetail>>(i => i.Count == 1 && i.First().ResourceId == "Q1"));
        repository.Received()
            .ExecuteAsync(Arg.Is<UpdateMatchCriteria>(c => c.MatchId == 123 && c.SelectedResources == "<Resources />" && c.StatusId == Status.Failure));
        logService.Received().WriteLog(123, Arg.Any<string>());
    }
}