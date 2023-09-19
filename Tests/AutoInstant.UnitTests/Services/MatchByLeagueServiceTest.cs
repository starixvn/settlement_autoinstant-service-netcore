using AutoInstant.Models;
using AutoInstant.Services;
using Fanex.Caching;
using Fanex.Caching.Redis;
using Microsoft.Extensions.Options;

namespace AutoInstant.UnitTests.Services;

public class MatchByLeagueServiceTest
{
    private readonly MatchQueryService matchQueryService;

    private readonly ICache cache;

    private readonly MatchByLeagueService matchByLeagueService;

    private static List<Match> CreateMatchesWithLeagueOne(params int[] matchIds)
    {
        const int leagueId = 58339;
        return matchIds.Select(i => new Match { MatchId = i, LeagueId = leagueId }).ToList();
    }

    private static List<Match> CreateMatchesWithLeagueTwo(params int[] matchIds)
    {
        const int leagueId = 45793;
        return matchIds.Select(i => new Match { MatchId = i, LeagueId = leagueId }).ToList();
    }

    private static List<Match> CreateMatchesWithLeagueThree(params int[] matchIds)
    {
        const int leagueId = 58087;
        return matchIds.Select(i => new Match { MatchId = i, LeagueId = leagueId }).ToList();
    }

    private static List<Match> CreateMatches
    {
        get
        {
            var matchesOne = CreateMatchesWithLeagueOne(1);
            var matchesTwo = CreateMatchesWithLeagueTwo(2);
            var matchesThree = CreateMatchesWithLeagueThree(3);

            var list = new List<Match>();
            list.AddRange(matchesOne);
            list.AddRange(matchesTwo);
            list.AddRange(matchesThree);

            return list;
        }
    }

    public MatchByLeagueServiceTest()
    {
        matchQueryService = Substitute.For<MatchQueryService>();
        cache = new RedisCache(new RedisCacheOptions
        {
            ConnectionString = "10.18.200.131:16379,password=foo1@bared,defaultDatabase=11",
            ApplicationName = "Settlement-LOCAL"
        });

        var settings = new AppSettingsKey()
        {
            RetryInterval = 1000,
            RetryCount = 1,
            EnvironmentName = "UAT"
        };
        var appSettingsOptions = Options.Create(settings);
        matchByLeagueService = new MatchByLeagueServiceImp(matchQueryService, cache, appSettingsOptions);
    }

    [Fact]
    public async Task GetMatches_ExistedIncludeLeagueIds_ReturnsRightMatches()
    {
        // Arrange
        const string keyLeagueId = "Settlement-AutoPayout-AutoInstant-LeagueId";
        const int cacheExpiredInDays = 1095;
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(cacheExpiredInDays));

        await cache.SetAsync(keyLeagueId, 58087, cachePolicy);

        matchQueryService.GetWaitingMatches().Returns(CreateMatches);

        // Act
        var matches = await matchByLeagueService.GetMatches();

        // Assert
        Assert.Single(matches);
    }
}