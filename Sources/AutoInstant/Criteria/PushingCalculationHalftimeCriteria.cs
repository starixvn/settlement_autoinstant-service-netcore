using AutoInstant.Models;

namespace AutoInstant.Criteria
{
    public class PushingCalculationHalftimeCriteria : PushingCalculationCriteria
    {
        public PushingCalculationHalftimeCriteria(Match match) : base(match)
        {
            Mode = "INST-HT";
        }
    }
}