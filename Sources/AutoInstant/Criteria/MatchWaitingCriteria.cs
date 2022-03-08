using Fanex.Data.Repository;

namespace AutoInstant.Criteria
{
    public class MatchWaitingCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "NAP_Set_AutoInstant_ScheduleSel";
        }
    }
}