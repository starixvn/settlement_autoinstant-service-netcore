using AutoInstant.Basketball.Criteria;
using AutoInstant.Basketball.Models;
using AutoInstant.Basketball.Services;
using Fanex.Data.Repository;

namespace AutoInstant.Basketball.UnitTests.Services;

public class ScoreValidateServiceTests
{
    private readonly IDynamicRepository repository;

    private readonly ScoreValidateService service;

    public ScoreValidateServiceTests()
    {
        repository = Substitute.For<IDynamicRepository>();
        service = new ScoreValidateServiceImp(repository);
    }

    [Fact]
    public void IsScoreDecreased_SetValidStatusResultDetail()
    {
        var resultDetails = new List<ResultDetail>
        {
            new()
            {
                ResourceId = "Q1",
                HomeValue = 1,
                AwayValue = 0
            },
            new()
            {
                ResourceId = "Q2",
                HomeValue = 1,
                AwayValue = 1
            },new()
            {
                ResourceId = "Q3",
                HomeValue = 1,
                AwayValue = 4
            },
        };
        repository.FetchAsync<Score>(Arg.Any<GetLatestScoreCriteria>()).Returns(new List<Score>
         {
             new() { ResourceId = "Q1", HomeValue = 1, AwayValue = 1 },
             new() { ResourceId = "Q3", HomeValue = 1, AwayValue = 4 }
         });

        service.IsScoreDecreased(new Match { ResultDetails = resultDetails });

        Assert.True(resultDetails[0].Invalid);
        Assert.False(resultDetails[1].Invalid);
        Assert.False(resultDetails[2].Invalid);
    }
}