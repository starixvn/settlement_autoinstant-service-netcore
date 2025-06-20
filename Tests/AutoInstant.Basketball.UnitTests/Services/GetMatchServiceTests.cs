using AutoInstant.Basketball.Constants;
using AutoInstant.Basketball.Models;
using AutoInstant.Basketball.Services;
using Fanex.Caching;
using Fanex.Caching.Redis;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;

namespace AutoInstant.Basketball.UnitTests.Services;

public class GetMatchServiceTests
{
    private readonly ICache cache = new RedisCache(new RedisCacheOptions
    {
        ConnectionString = "10.18.200.195:16379,password=foo1@bared,defaultDatabase=11",
        ApplicationName = "Settlement-LOCAL"
    });

    private readonly IDynamicRepository repository = Substitute.For<IDynamicRepository>();

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

    [Fact]
    public async Task GetMatches_NotExistLeagueTest_ReturnsAllMatches()
    {
        // Arrange
        var settings = new AppSettingsBasketballKey
        {
            EnvironmentName = "Local"
        };
        var appSettingsOptions = Options.Create(settings);

        var service = new GetMatchServiceImp(repository, cache, appSettingsOptions);

        repository.FetchAsync<ResultDetail>(Arg.Any<CriteriaBase>()).Returns(CreateResultDetails);

        // Act
        var matches = await service.GetMatches();

        // Assert
        Assert.Equal(5, matches.Count);
    }

    [Fact]
    public async Task GetMatches_ExcludeLeagueIdAndInUAT_ReturnsOneMatches()
    {
        // Arrange
        const int cacheExpiredInDays = 1095;
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(cacheExpiredInDays));
        await cache.SetAsync(CacheKey.LeagueId, "58087", cachePolicy);
        var settings = new AppSettingsBasketballKey
        {
            EnvironmentName = "UAT"
        };
        var appSettingsOptions = Options.Create(settings);

        var service = new GetMatchServiceImp(repository, cache, appSettingsOptions);

        repository.FetchAsync<ResultDetail>(Arg.Any<CriteriaBase>()).Returns(CreateResultDetails);

        // Act
        var matches = await service.GetMatches();

        // Assert
        Assert.Single(matches);
    }

    [Fact]
    public async Task GetMatches_ExcludeLeagueIdAndInPRO_ReturnsTwoMatches()
    {
        // Arrange
        const int cacheExpiredInDays = 1095;
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(cacheExpiredInDays));
        await cache.SetAsync(CacheKey.LeagueId, "58087", cachePolicy);
        var settings = new AppSettingsBasketballKey
        {
            EnvironmentName = "PRO"
        };
        var appSettingsOptions = Options.Create(settings);

        var service = new GetMatchServiceImp(repository, cache, appSettingsOptions);

        repository.FetchAsync<ResultDetail>(Arg.Any<CriteriaBase>()).Returns(CreateResultDetails);

        // Act
        var matches = await service.GetMatches();

        // Assert
        Assert.Equal(4, matches.Count);
    }
}