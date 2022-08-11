using Fanex.Data.Repository;

namespace AutoInstant.Criteria
{
    public class UpdateResultCriteria : NonQueryCommand
    {
        public int MatchId { get; set; }

        public int SourceId { get; set; } = 5;

        public int TypeId { get; set; }

        public string HomeValue { get; set; }

        public string AwayValue { get; set; }

        public int StatusId { get; set; } = 1;

        public int InstantKey { get; set; }

        public int Errcode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Set_AutoPayoutResultIns";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}