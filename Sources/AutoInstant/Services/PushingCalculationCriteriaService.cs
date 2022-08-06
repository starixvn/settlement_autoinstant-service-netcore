using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;

namespace AutoInstant.Services
{
    public interface PushingCalculationCriteriaService
    {
        PushingCalculationCriteria CreateCriteria(Match match, string resourceResult);
    }

    public class PushingCalculationCriteriaServiceImp : PushingCalculationCriteriaService
    {
        public PushingCalculationCriteria CreateCriteria(Match match, string resourceResult)
        {
            return match.TypeId == SettleType.FirstHalf
                ? new PushingCalculationHalftimeCriteria(match, resourceResult)
                : new PushingCalculationFullTimeCriteria(match, resourceResult);
        }
    }
}