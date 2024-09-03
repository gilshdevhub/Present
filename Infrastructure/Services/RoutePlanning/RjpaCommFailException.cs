namespace Infrastructure.Services.RoutePlanning;

public class RjpaCommFailException : Exception
{
    public RjpaCommFailException() : base()
    {
    }

    public RjpaCommFailException(string message) : base(message)
    {
    }

    public RjpaCommFailException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
