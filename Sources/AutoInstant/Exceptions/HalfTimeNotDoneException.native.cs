namespace AutoInstant.Exceptions;

[Serializable]
public class HalfTimeNotDoneException : AutoInstantException
{
    public HalfTimeNotDoneException()
    {
        Description = "Failed: Cannot execute FT auto instant calculation due to HT process (auto/manual) has not done yet.";
    }

    public HalfTimeNotDoneException(string message)
        : base(message)
    {
    }

    public HalfTimeNotDoneException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}