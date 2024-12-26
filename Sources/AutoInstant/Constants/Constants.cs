namespace AutoInstant.Constants;

public static class MatchStatus
{
    public const int Waiting = 1;

    public const int Completed = 2;

    public const int Failed = 3;
}

public static class ModeType
{
    public const string FirstHalf = "INST-HT";

    public const string SecondHalf = "INST-FT";
}

public static class SettleAction
{
    public const int ShowReferenceActionsForFirstHalf = 195;

    public const int ShowReferenceActionsForSecondHalf = 196;

    public const int HideReferenceActionsForFirstHalf = 197;

    public const int HideReferenceActionsForSecondHalf = 198;
}

public static class SettleType
{
    public const int FirstHalf = 101;

    public const int SecondHalf = 102;
}

public static class Source
{
    public const int AdminSite = 100;
}

public static class CacheKey
{
    public const string LeagueId = "Settlement-AutoPayout-AutoInstant-LeagueId";
}