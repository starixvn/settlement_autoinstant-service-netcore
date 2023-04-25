using AutoInstant.Constants;
using AutoInstant.Models;

namespace AutoInstant.Criteria;

public class PushingCalculationHalftimeCriteria : PushingCalculationCriteria
{
	public PushingCalculationHalftimeCriteria(Match match, string resourceResult) : base(match)
	{
		Action = $"CAL-{ModeType.FirstHalf}";
		//Mode = null;
		ResourceResult = resourceResult;
	}
}