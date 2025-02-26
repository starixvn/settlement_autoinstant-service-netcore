namespace AutoInstant.Exceptions;

[Serializable]
public class ResultOnHoldException : AutoInstantException
{
    public ResultOnHoldException()
    {
        IsShowOnReferenceActions = true;
        Description = "Failed Instant: HT or FT is on-hold.";
    }

    public ResultOnHoldException(string message)
        : base(message)
    {
    }


    public ResultOnHoldException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
