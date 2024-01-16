namespace AutoInstant.Exceptions;

[Serializable]
public class SettingOffException : AutoInstantException
{
    public SettingOffException()
    {
        IsSendToLetsTalk = false;
        Description = "Failed: Cannot execute Auto Instant calculate due to setting turned off.";
    }

    public SettingOffException(string message)
        : base(message)
    {
    }

    public SettingOffException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    [Obsolete(DiagnosticId = "SYSLIB0051")]
    protected SettingOffException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}