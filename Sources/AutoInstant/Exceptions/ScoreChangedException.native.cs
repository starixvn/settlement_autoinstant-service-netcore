namespace AutoInstant.Exceptions;

[Serializable]
public class ScoreChangedException : AutoInstantException
{
    public ScoreChangedException()
    {
        IsShowOnReferenceActions = true;
        Description = "Failed: Detect score changed.";
    }

    public ScoreChangedException(string message)
        : base(message)
    {
    }

    public ScoreChangedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    [Obsolete(DiagnosticId = "SYSLIB0051")]
    protected ScoreChangedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}