namespace AutoInstant.Exceptions;

[Serializable]
public class FTCalculatedException : AutoInstantException
{
    public FTCalculatedException()
    {
        IsSendToLetsTalk = false;
        Description = "Failed: FT has been calculated already.";
    }

    public FTCalculatedException(string message)
        : base(message)
    {
    }

    public FTCalculatedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}