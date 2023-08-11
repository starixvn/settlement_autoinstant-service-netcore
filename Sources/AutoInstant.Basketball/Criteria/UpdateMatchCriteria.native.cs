using Fanex.Data.Repository;

namespace AutoInstant.Basketball.Criteria;

public class UpdateMatchCriteria : NonQueryCommand
{
    public int MatchId { get; set; }

    public string SelectedResources { get; set; }

    public int SourceId { get; set; } = 98;

    public int StatusId { get; set; }

    public override string GetSettingKey() => "NAP_Set_AutoPayoutResultOS_Update";

    public override bool IsValid() => MatchId > 0;
}