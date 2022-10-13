using AutoInstant.Constants;
using AutoInstant.Models;

namespace AutoInstant.Criteria;

public class PushingCalculationFullTimeCriteria : PushingCalculationCriteria
{
    public PushingCalculationFullTimeCriteria(Match match, string resourceResult) : base(match)
    {
        Mode = ModeType.SecondHalf;
        ResourceResult = resourceResult;
    }
}