namespace AutoInstant.Result.Services;

public interface FirstGoalService
{
    string FirstGoal(string goalRecord);
}

public class FirstGoalServiceImp : FirstGoalService
{
    private const string NoGoal = "N";

    public string FirstGoal(string goalRecord)
    {
        return string.IsNullOrWhiteSpace(goalRecord) ? NoGoal : goalRecord[..1].ToUpperInvariant();
    }
}