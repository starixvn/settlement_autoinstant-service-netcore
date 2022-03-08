using AutoInstant.Constants;
using AutoInstant.Models;
using AutoInstant.Services;
using Fanex.Data.Repository;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AutoInstant.UnitTests.Services
{
    
    public class ScoreServiceTests
    {
        private readonly IDynamicRepository repository;

        public ScoreServiceTests()
        {
            repository = Substitute.For<IDynamicRepository>();
        }

        [Fact]
        public async Task GetScore_SourceIsNotSoccerWay_ReturnsEmpty()
        {
            // Arrange
            const int matchId = 1;
            var fakeScores = new List<Score>
            {
                new Score { SourceId = 2, TypeId = SettleType.FirstHalf  }
            };
            repository.Fetch<Score>(Arg.Any<ICriteria>()).Returns(fakeScores);

            var scoreService = new ScoreServiceImp(repository);
            // Act
            var scores = await scoreService.GetScore(matchId);

            // Assert
            Assert.Empty(scores);
        }

        [Theory]
        [InlineData(SettleType.FirstHalf)]
        [InlineData(SettleType.SecondHalf)]
        public async Task GetScore_Always_ReturnsNotNull(int typeId)
        {
            // Arrange
            const int matchId = 1;
            var fakeScores = new List<Score>
            {
                new Score { SourceId = Source.SoccerWay, TypeId = typeId  }
            };
            repository.Fetch<Score>(Arg.Any<ICriteria>()).Returns(fakeScores);

            var scoreService = new ScoreServiceImp(repository);
            // Act
            var scores = await scoreService.GetScore(matchId);

            // Assert
            Assert.NotNull(scores);
        }
    }
}