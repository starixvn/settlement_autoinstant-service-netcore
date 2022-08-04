using System;
using Kaden.Extensions;

namespace AutoInstant.Models
{
    public class MatchChecking
    {
        private const byte Instant = 2;

        private const int ToBase = 2;

        public byte AutoSetting { get; set; }

        public byte SettlementStatus { get; set; }

        public RemarkSettingModel Remark => new RemarkSettingModel
        {
            SettlementStatus = Convert.ToString(SettlementStatus, ToBase).Reverse(),
        };

        public int TypeId { get; set; }

        public int? HTHomeScore { get; set; }

        public int? HTAwayScore { get; set; }

        public string PreviousHomeScore { get; set; }

        public string PreviousAwayScore { get; set; }

        public int InstantKey { get; set; }

        public bool IsCalculatedFT { get; set; }

        public bool IsCalculatedHT { get; set; }

        public bool IsInstCalculated { get; set; }

        public string GoalRecord { get; set; }

        public bool IsSettingOff
        {
            get
            {
                return (AutoSetting & Instant) == 0;
            }
        }
    }
}