using AutoInstant.Models;
using AutoInstant.Services;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AutoInstant.UnitTests.Services
{
    
    public class MatchSettlingPoolServiceTests
    {
        private static List<Match> CreateMatches(params int[] matchIds)
        {
            return matchIds.Select(i => new Match { MatchId = i }).ToList();
        }

        [Fact]
        public void PeekNewMatch_InvokeDynamicRepositoryMethod()
        {
            // Arrange
            var emptyMatchList = new List<Match>();
            var matchByLeagueService = Substitute.For<MatchByLeagueService>();
            matchByLeagueService.GetMatches().Returns(emptyMatchList);
            var matchListingService = new MatchSettlingPoolServiceImp(matchByLeagueService);

            // Act
            matchListingService.PeekNewMatch();

            // Assert
            matchByLeagueService.Received().GetMatches();
        }

        [Fact]
        public void PeekNewMatch_Twice_OnlyAddNewMatchToPool()
        {
            // Arrange
            var matchByLeagueService = Substitute.For<MatchByLeagueService>();
            var matchListingService = new MatchSettlingPoolServiceImp(matchByLeagueService);

            // Act1
            matchByLeagueService.GetMatches().Returns(CreateMatches(1, 2));
            matchListingService.PeekNewMatch();

            // Act2
            matchByLeagueService.GetMatches().Returns(CreateMatches(2, 3));
            matchListingService.PeekNewMatch();

            // Assert
            Assert.Equal(3, matchListingService.PoolOfMatches.Count);
        }

        [Fact]
        public void Remove_ExistedMatchId_ExistingMatchIsRemove()
        {
            // Arrange
            var matchByLeagueService = Substitute.For<MatchByLeagueService>();
            var matchListingService = new MatchSettlingPoolServiceImp(matchByLeagueService);
            var matches = CreateMatches(1, 2);
            matchByLeagueService.GetMatches().Returns(matches);

            // Act1
            matchListingService.PeekNewMatch();
            matchListingService.Remove(matches[0]);

            // Assert
            Assert.Equal(1, matchListingService.PoolOfMatches.Count);
        }

        [Fact]
        public void Remove_NotExistedMatchId_NoChange()
        {
            // Arrange
            var matchByLeagueService = Substitute.For<MatchByLeagueService>();
            var matchListingService = new MatchSettlingPoolServiceImp(matchByLeagueService);
            var matches = CreateMatches(1, 2);
            matchByLeagueService.GetMatches().Returns(matches);

            // Act1
            matchListingService.PeekNewMatch();
            matchListingService.Remove(new Match { MatchId = 3 });

            // Assert
            Assert.Equal(2, matchListingService.PoolOfMatches.Count);
        }
    }
}