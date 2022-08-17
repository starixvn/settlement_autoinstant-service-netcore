using AutoInstant.Models;
using Fanex.Data;
using Fanex.Data.Repository;
using System;

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
            InstantKey = match.InstantKey;
        }

        public int AdminId { get; set; }

        public int MatchId { get; set; }

        public string BetStatus { get; set; }

        public int InstantKey { get; set; }

        public string HomeScore { get; set; }

        public string AwayScore { get; set; }

        public string Action { get; set; }

        public string Mode { get; set; }

        public string IP { get; set; }

        public string ResourceResult { get; set; }

        public bool IsAutoInst { get; set; }

        public string BroadcastSessionId = Guid.NewGuid().ToString();

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        [SpParam(Ignored = true)]
        public string DbServer { get; set; }

        public override string GetSettingKey()
        {
            return $"NAP_Admin_Settlement_1_InstantCalculation_Push_{DbServer}";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}