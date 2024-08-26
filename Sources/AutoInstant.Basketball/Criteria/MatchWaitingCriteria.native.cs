using Fanex.Data.Repository;

namespace AutoInstant.Basketball.Criteria;

public class MatchWaitingCriteria : CriteriaBase
{
    public string SportType { get; set; } = "2,8";

    public override bool IsValid() => true;

    public override string GetSettingKey() => "NAP_Set_AutoInstantOS_ScheduleSel";
}