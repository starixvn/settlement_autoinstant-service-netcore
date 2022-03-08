using AutoInstant.Models;
using AutoInstant.Services;
using Fanex.Caching;
using Fanex.Caching.Redis;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Xunit;

namespace AutoInstant.UnitTests.Services
{
    
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
                ConnectionString = $"ha.nexdev.net:16379,password=foo1@bared,defaultDatabase=11",
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
            const string KEY_LEAGUEID = "Settlement-AutoPayout-AutoInstant-LeagueId";
            const int CACHE_EXPIRED_IN_DAYS = 1095;
            var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CACHE_EXPIRED_IN_DAYS));
            cache.Set(KEY_LEAGUEID, 58087, cachePolicy);

            matchQueryService.GetWaitingMatches().Returns(CreateMatches);

            // Act
            var matches = await matchByLeagueService.GetMatches();

            // Assert
            Assert.Single(matches);
        }
    }
}