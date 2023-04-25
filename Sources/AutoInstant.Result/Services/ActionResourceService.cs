using AutoInstant.Result.Contants;
using AutoInstant.Result.Models;

namespace AutoInstant.Result.Services;

public interface ActionResourceService
{
	List<XmlResource> FullTimeExistCornerRaceResource(string halftimeGoal, string fullTimeGoal);
	XmlResource FullTimeFirstCornerResource(string halftimeGoal, string fullTimeGoal);
	XmlResource FullTimeFirstGoalResources(string halftimeGoal, string fullTimeGoal);
	XmlResource HafltimeFirstCornerResource(string halftimeGoal);
	List<XmlResource> HalftimeExistCornerRaceResource(string halftimeGoal);
	XmlResource HalftimeFirstGoalResources(string halftimeGoal);
	XmlResource TwoHFirstCornerResource(string fullTimeGoal);
}


public class ActionResourceServiceImp : ActionResourceService
{
	private const byte ManuallySettled = 5;
	private const byte Completed = 4;
	private const string HomeValue = "H";

	private readonly FirstGoalService goalResultService;

	public ActionResourceServiceImp(FirstGoalService goalResultService)
	{
		this.goalResultService = goalResultService;
	}

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

	public XmlResource HafltimeFirstCornerResource(string halftimeGoal)
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
			ResourceId = ResourceKey.FirstCorner,
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
			HomeValue = $"{v.value}",
			ResourceId = $"{ResourceKey.CornerRace}{ConvertIntToString(v.raceTo)}",
			ResultStatusId = ManuallySettled,
			SettlementStatusId = Completed,
		}).ToList();
	}


	public List<XmlResource> HalftimeExistCornerRaceResource(string halftimeGoal)
	{
		var result = ConvertGoalRecordToRace(halftimeGoal);

		return result.Select(v => new XmlResource
		{
			HomeValue = $"{v.value}",
			ResourceId = $"{ResourceKey.HalftimeCornerRace}{ConvertIntToString(v.raceTo)}",
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

	private static string ConvertIntToString(int num) => num < 10 ? $"0{num}" : $"{num}";

}