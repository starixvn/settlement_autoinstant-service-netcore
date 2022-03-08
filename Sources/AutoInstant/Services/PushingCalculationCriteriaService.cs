using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;

namespace AutoInstant.Services
{
    public interface PushingCalculationCriteriaService
    {
        PushingCalculationCriteria CreateCriteria(Match match);
    }

    public class PushingCalculationCriteriaServiceImp : PushingCalculationCriteriaService
    {
        public PushingCalculationCriteria CreateCriteria(Match match)
        {
            return match.TypeId == SettleType.FirstHalf
                ? (PushingCalculationCriteria)(new PushingCalculationHalftimeCriteria(match))
                : (PushingCalculationCriteria)(new PushingCalculationFullTimeCriteria(match));
        }
    }
}