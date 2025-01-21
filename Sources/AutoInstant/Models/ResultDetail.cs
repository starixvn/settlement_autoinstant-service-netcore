namespace AutoInstant.Models;
public class ResultDetail
{
    private const int ToBase = 2;

    private const int HoldResultPosition = 0;

    public string ResourceId { get; set; }

    public int SourceId { get; set; }

    public byte ResultSetting { get; set; }

    public bool IsHold
    {
        get
        {
            var value = Convert.ToString(ResultSetting, ToBase).Reverse();
            return !string.IsNullOrWhiteSpace(value) && value.Length > HoldResultPosition && value[HoldResultPosition].Equals('1');
        }
    }
}
