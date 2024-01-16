namespace AutoInstant.Exceptions;

[Serializable]
public class ScoreMismatchedException : AutoInstantException
{
    public ScoreMismatchedException()
    {
        IsShowOnReferenceActions = true;
        Description = "Failed: Suspender's score and Info Site's score are mismatched.";
    }

    public ScoreMismatchedException(string message)
        : base(message)
    {
    }

    public ScoreMismatchedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    [Obsolete(DiagnosticId = "SYSLIB0051")]
    protected ScoreMismatchedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}