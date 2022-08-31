using AutoInstant.Constants;
using Fanex.Data.Repository;

namespace AutoInstant.Criteria
{
    public class AddNotificationCriteria : NonQueryCommand
    {
        public int MessageGroupId { get; } = 1;

        public string MessageType { get; } = "Warning";

        public int MatchId { get; set; }

        public string ActionName { get; set; }

        public string MessageContent { get; set; }

        public int UserId { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid() => MatchId > 0;

        public override string GetSettingKey() => "NAP_Set_LetstalkMessage_Insert";
    }
}