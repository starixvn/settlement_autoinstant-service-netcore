using Fanex.Data;
using Fanex.Data.Repository;

namespace AutoInstant.Basketball.Criteria;

public class SettleProfileCriteria : NonQueryCommand
{
    public int MatchId { get; set; }

    public int AdminId { get; set; }

    public string SelectedResources { get; set; }

    public string Ip { get; set; } = string.Empty;

    public string Action { get; set; } = "CAL-INST";

    public int DeterminedType { get; set; }

    public string BroadcastSessionId { get; } = Guid.NewGuid().ToString();

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey() => $"NAP_Admin_Settlement_Profile_Instant_Push_{DbServer}";

    public override bool IsValid() => MatchId > 0;
}