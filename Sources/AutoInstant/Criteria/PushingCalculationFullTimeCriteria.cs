using AutoInstant.Models;

namespace AutoInstant.Criteria
{
    public class PushingCalculationFullTimeCriteria : PushingCalculationCriteria
    {
        public PushingCalculationFullTimeCriteria(Match match) : base(match)
        {
            Mode = "INST-FT";
        }
    }
}