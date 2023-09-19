using AutoInstant.Basketball.Models;
using AutoInstant.Basketball.Services;
using Fanex.Caching;
using Fanex.Caching.Redis;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;

namespace AutoInstant.Basketball.UnitTests.Services;

public class GetMatchServiceTests
{
    private readonly ICache cache;

    private readonly IDynamicRepository repository;

    private readonly GetMatchService service;

    private static IEnumerable<ResultDetail> CreateResultDetailsWithLeagueOne(IEnumerable<int> resultDetailIds)
    {
        const int leagueId = 58339;
        return resultDetailIds.Select(i => new ResultDetail { MatchId = i, LeagueId = leagueId }).ToList();
    }

    private static IEnumerable<ResultDetail> CreateResultDetailsWithLeagueTwo(IEnumerable<int> resultDetailIds)
    {
        const int leagueId = 45793;
        return resultDetailIds.Select(i => new ResultDetail { MatchId = i, LeagueId = leagueId }).ToList();
    }

    private static IEnumerable<ResultDetail> CreateResultDetailsWithLeagueThree(IEnumerable<int> resultDetailIds)
    {
        const int leagueId = 58087;
        return resultDetailIds.Select(i => new ResultDetail { MatchId = i, LeagueId = leagueId }).ToList();
    }

    private static List<ResultDetail> CreateResultDetails
    {
        get
        {
            var resultDetailsOne = CreateResultDetailsWithLeagueOne(new List<int> { 1, 2 });
            var resultDetailsTwo = CreateResultDetailsWithLeagueTwo(new List<int> { 3, 4 });
            var resultDetailsThree = CreateResultDetailsWithLeagueThree(new List<int> { 5 });

            var list = new List<ResultDetail>();
            list.AddRange(resultDetailsOne);
            list.AddRange(resultDetailsTwo);
            list.AddRange(resultDetailsThree);

            return list;
        }
    }

    public GetMatchServiceTests()
    {
        cache = new RedisCache(new RedisCacheOptions
        {
            ConnectionString = "10.18.200.131:16379,password=foo1@bared,defaultDatabase=11",
            ApplicationName = "Settlement-LOCAL"
        });

        var settings = new AppSettingsBasketballKey()
        {
            EnvironmentName = "UAT"
        };
        repository = Substitute.For<IDynamicRepository>();
        var appSettingsOptions = Options.Create(settings);
        service = new GetMatchServiceImp(repository, cache, appSettingsOptions);
    }

    [Fact]
    public async Task GetMatches_ExistedIncludeLeagueIds_ReturnsRightMatches()
    {
        // Arrange
        const string keyLeagueId = "Settlement-AutoPayoutOtherSport-LeagueId";
        const int cacheExpiredInDays = 1095;
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(cacheExpiredInDays));
        await cache.SetAsync(keyLeagueId, "58087", cachePolicy);

        repository.FetchAsync<ResultDetail>(Arg.Any<CriteriaBase>()).Returns(CreateResultDetails);

        // Act
        var matches = await service.GetMatches();

        // Assert
        Assert.Single(matches);
    }
}