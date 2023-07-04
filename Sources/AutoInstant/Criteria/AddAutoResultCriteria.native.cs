using AutoInstant.Constants;

namespace AutoInstant.Criteria;

public class AddAutoResultCriteria : NonQueryCommand
{
    public int MatchId { get; init; }

    public int SourceId { get; set; } = Source.SoccerWay;

    public int TypeId { get; set; }

    public int HomeValue { get; set; }

    public int AwayValue { get; set; }

    public int StatusId { get; set; }

    public int InstantKey { get; set; }

    public int ChildMatchType { get; set; }

    public bool IsInstCalculated { get; init; }

    public override bool IsValid() => MatchId > 0;

    public override string GetSettingKey() => "NAP_Set_AutoPayoutResultIns";
}