using Fanex.Data.Repository;

namespace AutoInstant.Basketball.Criteria;

public class LogCriteria : NonQueryCommand
{
    public LogCriteria(int matchId, string remark)
    {
        MatchId = matchId;
        Remark = remark;
    }

    public int UserId { get; set; }

    public int MatchId { get; set; }

    public int ActionId { get; set; } = 81042;

    public string Remark { get; set; }

    public string Ip { get; } = string.Empty;

    public override bool IsValid() => MatchId > 0 && ActionId > 0;

    public override string GetSettingKey() => "WriteLog_UserLog_Settle";
}