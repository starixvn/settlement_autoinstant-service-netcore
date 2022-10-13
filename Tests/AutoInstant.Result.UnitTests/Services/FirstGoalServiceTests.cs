using AutoInstant.Result.Services;

namespace AutoInstant.Result.UnitTests.Services;

public class FirstGoalServiceTests
{
    private readonly FirstGoalService service;

    public FirstGoalServiceTests()
    {
        service = new FirstGoalServiceImp();
    }

    [Theory]
    [InlineData("", "N")]
    [InlineData("ah", "A")]
    public void FirstGoal_Always_ReturnsExpectedGoal(string goalRecord, string expectedGoal)
    {
        // Act
        var actual = service.FirstGoal(goalRecord);

        // Assert
        Assert.Equal(expectedGoal, actual);
    }
}