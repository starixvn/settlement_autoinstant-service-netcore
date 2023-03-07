namespace AutoInstant.Criteria;
public class LogDirectlyCriteria : NonQueryCommand
{
    public int UserID { get; set; }
    public string Doing { get; set; }
    public int Extend { get; set; }

    public override string GetSettingKey() => "NAP_UserLog_WriteLog";
    public override bool IsValid() => true;
}
