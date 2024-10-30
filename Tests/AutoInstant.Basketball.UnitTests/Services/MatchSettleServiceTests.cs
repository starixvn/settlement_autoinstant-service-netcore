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
        var settings = new AppSettingsBasketballKey
        {
            SiteDbServer = ["site1", "site2"]
        };
        var appSettingsOptions = Options.Create(settings);

        service = new MatchSettleServiceImp(repository, appSettingsOptions, logService, generateService);
    }

    [Fact]
    public void Settle_ExistResultDetails_UpdateAndSettleResult()
    {
        var resultDetails = new List<ResultDetail> { new() { ResourceId = "Q1", Invalid = false } };
        var match = new Match
        {
            MatchId = 123,
            ResultDetails = resultDetails
        };

        generateService.CreateResources(Arg.Is<List<ResultDetail>>(i => i.Count == 1 && i[0].ResourceId == "Q1"))
            .Returns("<Resources />");
        service.Settle(match);

        generateService.Received().CreateResources(Arg.Is<List<ResultDetail>>(i => i.Count == 1 && i[0].ResourceId == "Q1"));
        repository.Received()
            .ExecuteAsync(Arg.Is<UpdateMatchCriteria>(c => c.MatchId == 123 && c.SelectedResources == "<Resources />" && c.StatusId == Status.Waiting));
        logService.Received().WriteDirectlyLog(Arg.Any<SettleProfileCriteria>());
        repository.Received(2).ExecuteAsync(Arg.Any<SettleProfileCriteria>());
    }

    [Fact]
    public void HandleScoreFailed_ExistResultDetails_UpdateAndLogResult()
    {
        var resultDetails = new List<ResultDetail> { new() { ResourceId = "Q1", Invalid = true } };
        var match = new Match
        {
            MatchId = 123,
            ResultDetails = resultDetails
        };

        generateService.CreateResources(Arg.Is<List<ResultDetail>>(i => i.Count == 1 && i[0].ResourceId == "Q1"))
            .Returns("<Resources />");
        service.HandleScoreFailed(match);

        generateService.Received().CreateResources(Arg.Is<List<ResultDetail>>(i => i.Count == 1 && i[0].ResourceId == "Q1"));
        repository.Received()
            .ExecuteAsync(Arg.Is<UpdateMatchCriteria>(c => c.MatchId == 123 && c.SelectedResources == "<Resources />" && c.StatusId == Status.Failure));
        logService.Received().WriteLog(123, Arg.Any<string>());
    }
}