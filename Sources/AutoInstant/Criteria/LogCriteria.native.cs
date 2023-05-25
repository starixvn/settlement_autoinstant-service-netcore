using AutoInstant.Models;

namespace AutoInstant.Criteria;

public class LogCriteria : NonQueryCommand
{
    public LogCriteria(Match match)
    {
        MatchId = match.MatchId;
    }

    public int UserId { get; }

    public int MatchId { get; set; }

    public int ActionId { get; set; }

    public string Remark { get; set; }

    public string GoalRecord { get; set; }

    public string Ip { get; } = string.Empty;

    public override bool IsValid() => MatchId > 0 && ActionId > 0;

    public override string GetSettingKey() => "WriteLog_UserLog_Settle";
}