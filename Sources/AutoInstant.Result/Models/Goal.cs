namespace AutoInstant.Result.Models;

public class Goal
{
	public bool IsExistHtFgNg { get; set; }

	public bool IsExistFgNg { get; set; }

	public bool IsExistHTFG { get; set; }

	public bool IsExistFG { get; set; }

	public bool IsExistCorner { get; set; }
	public bool IsExistHTCorner { get; set; }
	public bool IsExistFirstCorner { get; set; }
	public bool IsExistHTFirstCorner { get; set; }
	public bool IsExist2HFirstCorner { get; set; }

	public string GoalRecord { get; set; }
}