namespace AutoInstant.Exceptions;

[Serializable]
public class AutoInstantException : Exception
{
    public AutoInstantException()
    {
    }

    public AutoInstantException(string message)
        : base(message)
    {
    }

    public AutoInstantException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public string Description { get; set; }

    public bool IsShowOnReferenceActions { get; set; }

    public bool IsSendToLetsTalk { get; set; } = true;
}