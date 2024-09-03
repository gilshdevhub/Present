using System.Runtime.Serialization;

namespace Infrastructure.Services.Notifications;

public class NotificationException : Exception
{
    public NotificationException() : base()
    {
    }

    public NotificationException(string message) : base(message)
    {
    }

    public NotificationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public NotificationException(string message, string body, Exception innerException) : this(message, innerException)
    {
        Body = body;
    }

    protected NotificationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
    {
        throw new NotImplementedException();
    }

    public string Body { get; }
}
