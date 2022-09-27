using Fanex.Data;
using Fanex.Data.Repository;

namespace AutoInstant.Criteria
{
    public class MatchTicketCheckingCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        [SpParam(Ignored = true)]
        public string DbServer { get; set; }

        public override bool IsValid() => MatchId > 0;

        public override string GetSettingKey() => $"NAP_Set_AutoInstant_GetTicketInfo_{DbServer}";
    }
}