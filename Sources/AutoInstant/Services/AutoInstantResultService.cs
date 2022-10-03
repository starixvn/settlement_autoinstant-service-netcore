using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using MatchType = AutoInstant.Constants.MatchType;

namespace AutoInstant.Services;

public interface AutoInstantResultService
{
    Task Update(Match match, bool isInstCalculated, int statusId);

    Task Insert(Match match, MatchChecking matchChecking);
}

public class AutoInstantResultServiceImp : AutoInstantResultService
{
    private readonly IDynamicRepository repository;

    public AutoInstantResultServiceImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task Update(Match match, bool isInstCalculated, int statusId)
    {
        var updateResultCriteria = new AddAutoResultCriteria
        {
            MatchId = match.MatchId,
            HomeValue = match.HomeValue,
            AwayValue = match.AwayValue,
            TypeId = match.TypeId,
            InstantKey = match.InstantKey,
            IsInstCalculated = isInstCalculated,
            ChildMatchType = match.ChildMatchType,
            StatusId = statusId,
            SourceId = match.ChildMatchType == MatchType.Corner ? Source.AdminSite : Source.SoccerWay
        };

        await repository.ExecuteAsync(updateResultCriteria);
    }

    public async Task Insert(Match match, MatchChecking matchChecking)
    {
        var isInstCalculated = IsManualInstCalculated(match, matchChecking);
        if (!IsCalculated(match, matchChecking))
        {
            await Update(match, isInstCalculated, MatchStatus.Failed);
        }
    }

    private static bool IsCalculated(Match match, MatchChecking matchChecking)
    {
        return IsFullTimeCalculated(match, matchChecking);
    }

    private static bool IsManualInstCalculated(Match match, MatchChecking matchChecking)
    {
        return matchChecking.IsInstCalculated && IsRightScore(match, matchChecking);
    }

    private static bool IsRightScore(Match match, MatchChecking matchChecking)
    {
        return match.HomeValue == Convert.ToInt32(matchChecking.PreviousHomeScore)
            && Convert.ToInt32(match.AwayValue) == Convert.ToInt32(matchChecking.PreviousAwayScore);
    }

    private static bool IsFullTimeCalculated(Match match, MatchChecking matchChecking)
    {
        return match.TypeId == SettleType.SecondHalf && matchChecking.IsCalculatedFT;
    }
}