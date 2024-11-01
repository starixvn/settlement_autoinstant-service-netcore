using AutoInstant.Constants;
using AutoInstant.Models;
using AutoInstant.Services;
using Fanex.Data.Repository;

namespace AutoInstant.UnitTests.Services;

public class ScoreServiceTests
{
    private readonly IDynamicRepository repository = Substitute.For<IDynamicRepository>();

    [Theory]
    [InlineData(SettleType.FirstHalf)]
    [InlineData(SettleType.SecondHalf)]
    public async Task GetScore_Always_ReturnsNotNull(int typeId)
    {
        // Arrange
        const int matchId = 1;
        var fakeScores = new List<Score>
        {
            new () { TypeId = typeId  }
        };
        repository.Fetch<Score>(Arg.Any<ICriteria>()).Returns(fakeScores);

        var scoreService = new ScoreServiceImp(repository);
        // Act
        var scores = await scoreService.GetScore(matchId);

        // Assert
        Assert.NotNull(scores);
    }
}