using Fanex.Data.Repository;

namespace AutoInstant.Criteria
{
    public class MatchScoreCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "NAP_Set_AutoPayout_ResultDetailByMatchIdSel";
        }
    }
}