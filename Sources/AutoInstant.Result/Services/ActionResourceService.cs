using AutoInstant.Result.Contants;
using AutoInstant.Result.Models;

namespace AutoInstant.Result.Services;

public interface ActionResourceService
{
    List<XmlResource> FullTimeExistCornerRaceResource(string halftimeGoal, string fullTimeGoal);

    XmlResource FullTimeFirstCornerResource(string halftimeGoal, string fullTimeGoal);

    XmlResource FullTimeFirstGoalResources(string halftimeGoal, string fullTimeGoal);

    XmlResource HalftimeFirstCornerResource(string halftimeGoal);

    List<XmlResource> HalftimeExistCornerRaceResource(string halftimeGoal);

    XmlResource HalftimeFirstGoalResources(string halftimeGoal);

    XmlResource TwoHFirstCornerResource(string fullTimeGoal);
}

public class ActionResourceServiceImp(FirstGoalService goalResultService) : ActionResourceService
{
    private const byte ManuallySettled = 5;
    private const byte Completed = 4;
    private const char HomeValue = 'H';
    private const int Ten = 10;

    public XmlResource HalftimeFirstGoalResources(string halftimeGoal)
    {
        var firstGoal = goalResultService.FirstGoal(halftimeGoal);

        return new XmlResource
        {
            ResourceId = ResourceKey.HalftimeFirstGoal,
            ResultStatusId = ManuallySettled,
            SettlementStatusId = Completed,
            HomeValue = firstGoal
        };
    }

    public XmlResource HalftimeFirstCornerResource(string halftimeGoal)
    {
        var firstGoal = goalResultService.FirstGoal(halftimeGoal);

        return new XmlResource
        {
            ResourceId = ResourceKey.HaftTimeFirstCorner,
            ResultStatusId = ManuallySettled,
            SettlementStatusId = Completed,
            HomeValue = firstGoal,
        };
    }

    public XmlResource FullTimeFirstCornerResource(string halftimeGoal, string fullTimeGoal)
    {
        var goal = $"{halftimeGoal}{fullTimeGoal}";
        var firstGoal = goalResultService.FirstGoal(goal);

        return new XmlResource
        {
            ResourceId = ResourceKey.FirstCorner,
            ResultStatusId = ManuallySettled,
            SettlementStatusId = Completed,
            HomeValue = firstGoal,
        };
    }

    public XmlResource TwoHFirstCornerResource(string fullTimeGoal)
    {
        var firstGoal = goalResultService.FirstGoal(fullTimeGoal);

        return new XmlResource
        {
            ResourceId = ResourceKey.TowHFirstCorner,
            ResultStatusId = ManuallySettled,
            SettlementStatusId = Completed,
            HomeValue = firstGoal,
        };
    }

    public List<XmlResource> FullTimeExistCornerRaceResource(string halftimeGoal, string fullTimeGoal)
    {
        var goal = $"{halftimeGoal}{fullTimeGoal}";
        var result = ConvertGoalRecordToRace(goal);

        return result.Select(v => new XmlResource
        {
            HomeValue = $"{v.Value}",
            ResourceId = $"{ResourceKey.CornerRace}{PadLeft(v.RaceTo)}",
            ResultStatusId = ManuallySettled,
            SettlementStatusId = Completed,
        }).ToList();
    }

    public List<XmlResource> HalftimeExistCornerRaceResource(string halftimeGoal)
    {
        var result = ConvertGoalRecordToRace(halftimeGoal);

        return result.Select(v => new XmlResource
        {
            HomeValue = $"{v.Value}",
            ResourceId = $"{ResourceKey.HalftimeCornerRace}{PadLeft(v.RaceTo)}",
            ResultStatusId = ManuallySettled,
            SettlementStatusId = Completed,
        }).ToList();
    }

    public XmlResource FullTimeFirstGoalResources(string halftimeGoal, string fullTimeGoal)
    {
        var goal = $"{halftimeGoal}{fullTimeGoal}";
        var firstGoal = goalResultService.FirstGoal(goal);

        return new XmlResource
        {
            ResourceId = ResourceKey.FullTimeFirstGoal,
            ResultStatusId = ManuallySettled,
            SettlementStatusId = Completed,
            HomeValue = firstGoal
        };
    }

    private static List<GoalRecordCorner> ConvertGoalRecordToRace(string goalRecord)
    {
        var homeRaceTo = 0;
        var awayRaceTo = 0;
        var result = new List<GoalRecordCorner>();

        foreach (var item in goalRecord)
        {
            if (item.Equals(HomeValue))
            {
                homeRaceTo = homeRaceTo + 1;
                if (homeRaceTo > awayRaceTo)
                {
                    result.Add(new GoalRecordCorner(item, homeRaceTo));
                }
            }
            else
            {
                awayRaceTo = awayRaceTo + 1;
                if (awayRaceTo > homeRaceTo)
                {
                    result.Add(new GoalRecordCorner(item, awayRaceTo));
                }
            }
        }

        return result;
    }

    private static string PadLeft(int num) => num < Ten ? $"0{num}" : $"{num}";
}