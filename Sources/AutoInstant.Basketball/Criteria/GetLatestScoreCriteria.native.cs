using Fanex.Data.Repository;

namespace AutoInstant.Basketball.Criteria;

public class GetLatestScoreCriteria : CriteriaBase
{
    public int MatchId { get; set; }

    public string ResourceIds { get; set; }

    public override string GetSettingKey()
    {
        return "NAP_Set_AutoInstantOS_ResultSel";
    }

    public override bool IsValid()
    {
        return MatchId > 0;
    }
}