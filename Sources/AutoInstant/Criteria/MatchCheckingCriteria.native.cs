using Fanex.Data.Repository;

namespace AutoInstant.Criteria
{
    public class MatchCheckingCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public int InstantKey { get; set; }

        public string HomeScore { get; set; }

        public string AwayScore { get; set; }

        public override bool IsValid() => MatchId > 0;

        public override string GetSettingKey() => "NAP_Set_AutoInstant_GetMatchInfo";
    }
}