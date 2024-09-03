namespace Infrastructure.Services.RoutePlanning;

public class FreeSeatsCommException : Exception
{
    public FreeSeatsCommException() : base()
    {
    }

    public FreeSeatsCommException(string message) : base(message)
    {
    }

    public FreeSeatsCommException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
