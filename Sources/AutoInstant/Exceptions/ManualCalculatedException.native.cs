namespace AutoInstant.Exceptions;

[Serializable]
public class ManualCalculatedException : AutoInstantException
{
    public ManualCalculatedException()
    {
    }

    public ManualCalculatedException(string liveScore, bool isSendToLetsTalk)
    {
        IsSendToLetsTalk = isSendToLetsTalk;
        Description = $"Failed: Score {liveScore} has been calculated already.";
    }

    public ManualCalculatedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    [Obsolete(DiagnosticId = "SYSLIB0051")]
    protected ManualCalculatedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}