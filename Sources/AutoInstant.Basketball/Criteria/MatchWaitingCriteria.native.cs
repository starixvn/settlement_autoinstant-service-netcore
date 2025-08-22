using Fanex.Data.Repository;

namespace AutoInstant.Basketball.Criteria;

public class MatchWaitingCriteria : CriteriaBase
{
    public int SportType => 2;

    public override bool IsValid() => true;

    public override string GetSettingKey() => "NAP_Set_AutoInstantOS_ScheduleSel";
}
