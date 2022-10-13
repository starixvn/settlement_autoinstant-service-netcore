namespace AutoInstant.Exceptions;

[Serializable]
public class RemarkException : AutoInstantException
{
    public RemarkException()
    {
        IsSendToLetsTalk = false;
        Description = "Failed: Cannot execute auto-instant-settle process due to match is on hold.";
    }

    public RemarkException(string message)
        : base(message)
    {
    }

    public RemarkException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected RemarkException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}