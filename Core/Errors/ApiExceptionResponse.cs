using System.Net;

namespace Core.Errors;

public class ApiExceptionResponse : ApiErrorResponse
{
    public ApiExceptionResponse(string message = null, string details = null) : base((int)HttpStatusCode.InternalServerError, message)
    {
        Details = details;
        Errors = new string[] { message };
    }

    public string Details { get; set; }
}
