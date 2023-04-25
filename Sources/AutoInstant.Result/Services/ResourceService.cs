using AutoInstant.Result.Models;

namespace AutoInstant.Result.Services;

public interface ResourceService
{
	List<XmlResource> Halftime(Goal goal);

	List<XmlResource> FullTime(Goal goal);
}

public class ResourceServiceImp : ResourceService
{
	private readonly ActionResourceService actionResourceService;

	private readonly GoalRecordService goalRecordService;

	public ResourceServiceImp(
		ActionResourceService actionResourceService,
		GoalRecordService goalRecordService)
	{
		this.actionResourceService = actionResourceService;
		this.goalRecordService = goalRecordService;
	}

	public List<XmlResource> Halftime(Goal goal)
	{
		var resources = new List<XmlResource>();
		var halftimeGoal = goalRecordService.HalftimeGoal(goal.GoalRecord);

		if (goal.IsExistHtFgNg)
		{
			var resource = actionResourceService.HalftimeFirstGoalResources(halftimeGoal);
			resources.Add(resource);
		}

		if (goal.IsExistFgNg)
		{
			var fullTimeGoal = goalRecordService.FullTimeGoal(goal.GoalRecord);
			var resource = actionResourceService.FullTimeFirstGoalResources(halftimeGoal, fullTimeGoal);
			resources.Add(resource);
		}

		if (goal.IsExistFirstCorner)
		{
			var fullTimeGoal = goalRecordService.FullTimeGoal(goal.GoalRecord);
			var resource = actionResourceService.FullTimeFirstCornerResource(halftimeGoal, fullTimeGoal);
			resources.Add(resource);
		}

		if (goal.IsExistHTFirstCorner)
		{
			var resource = actionResourceService.HafltimeFirstCornerResource(halftimeGoal);
			resources.Add(resource);
		}

		if (goal.IsExistHTCorner)
		{
			var resource = actionResourceService.HalftimeExistCornerRaceResource(halftimeGoal);
			resources.AddRange(resource);
		}

		if (goal.IsExistCorner)
		{
			var fullTimeGoal = goalRecordService.FullTimeGoal(goal.GoalRecord);
			var resource = actionResourceService.FullTimeExistCornerRaceResource(halftimeGoal, fullTimeGoal);
			resources.AddRange(resource);
		}

		return resources;
	}

	public List<XmlResource> FullTime(Goal goal)
	{
		var resources = new List<XmlResource>();
		var halftimeGoal = goalRecordService.HalftimeGoal(goal.GoalRecord);
		var fullTimeGoal = goalRecordService.FullTimeGoal(goal.GoalRecord);

		if (goal.IsExistFgNg)
		{
			var resource = actionResourceService.FullTimeFirstGoalResources(halftimeGoal, fullTimeGoal);
			resources.Add(resource);
		}

		if (goal.IsExistCorner)
		{
			var resource = actionResourceService.FullTimeExistCornerRaceResource(halftimeGoal, fullTimeGoal);
			resources.AddRange(resource);
		}

		if (goal.IsExistFirstCorner)
		{
			var resource = actionResourceService.FullTimeFirstCornerResource(halftimeGoal, fullTimeGoal);
			resources.Add(resource);
		}

		if (goal.IsExist2HFirstCorner)
		{
			var resource = actionResourceService.TwoHFirstCornerResource(fullTimeGoal);
			resources.Add(resource);
		}

		return resources;
	}
}