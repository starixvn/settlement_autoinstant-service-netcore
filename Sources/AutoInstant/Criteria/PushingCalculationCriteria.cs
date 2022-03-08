using AutoInstant.Models;
using Fanex.Data.Repository;

namespace AutoInstant.Criteria
{
    public class PushingCalculationCriteria : NonQueryCommand
    {
        public PushingCalculationCriteria(Match match)
        {
            AdminId = 0;
            ErrMsg = string.Empty;
            IP = string.Empty;
            IsAutoInst = true;
            Action = "CAL";
            MatchId = match.MatchId;
            HomeScore = match.HomeValue.ToString();
            AwayScore = match.AwayValue.ToString();
        }

        public int AdminId { get; set; }

        public int MatchId { get; set; }

        public int InstantKey { get; set; }

        public string HomeScore { get; set; }

        public string AwayScore { get; set; }

        public string Action { get; set; }

        public string Mode { get; set; }

        public string IP { get; set; }

        public string ResourceResult { get; set; }

        public bool IsAutoInst { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Admin_Settlement_1_InstantCalculation_Push";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}