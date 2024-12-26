namespace AutoInstant.Basketball.Constants;

public static class ErrorMessage
{
    public const string ScoreDecreased = "Failed Auto Instant: decreased score.";
}

public static class Status
{
    public const int Waiting = 1;

    public const int Success = 2;

    public const int Failure = 3;
}

public static class CacheKey
{
    public const string LeagueId = "Settlement-AutoPayoutOtherSport-LeagueId";
}