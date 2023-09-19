using AutoInstant.Basketball.Models;
using AutoInstant.Basketball.Services;

namespace AutoInstant.Basketball.UnitTests.Services;

public class MatchSettingPoolServiceTests
{
    private readonly GetMatchService getMatchService;
    private readonly MatchSettlingPoolServiceImp service;

    public MatchSettingPoolServiceTests()
    {
        getMatchService = Substitute.For<GetMatchService>();
        getMatchService.GetMatches().Returns(new List<Match>());
        service = new MatchSettlingPoolServiceImp(getMatchService);
    }

    private static List<Match> CreateMatches(params int[] matchIds)
    {
        return matchIds.Select(i => new Match { MatchId = i }).ToList();
    }

    [Fact]
    public async void PeekNewMatch_InvokeDynamicRepositoryMethod()
    {
        // Act
        await service.PeekNewMatch();

        // Assert
        await getMatchService.Received().GetMatches();
    }

    [Fact]
    public async void PeekNewMatch_Twice_OnlyAddNewMatchToPool()
    {
        // Act1
        getMatchService.GetMatches().Returns(CreateMatches(1, 2));
        await service.PeekNewMatch();

        // Act2
        getMatchService.GetMatches().Returns(CreateMatches(2, 3));
        await service.PeekNewMatch();

        // Assert
        Assert.Equal(3, service.PoolOfMatches.Count);
    }

    [Fact]
    public async void Remove_ExistedMatchId_ExistingMatchIsRemove()
    {
        // Arrange
        var matches = CreateMatches(1, 2);
        getMatchService.GetMatches().Returns(matches);

        // Act1
        await service.PeekNewMatch();
        service.Remove(matches[0]);

        // Assert
        Assert.Single(service.PoolOfMatches);
    }

    [Fact]
    public async void Remove_NotExistedMatchId_NoChange()
    {
        // Arrange
        var matches = CreateMatches(1, 2);
        getMatchService.GetMatches().Returns(matches);

        // Act1
        await service.PeekNewMatch();
        service.Remove(new Match { MatchId = 3 });

        // Assert
        Assert.Equal(2, service.PoolOfMatches.Count);
    }
}