using System.Net;

namespace Core.Errors;

public class ApiValidationResponse : ApiErrorResponse
{
    public ApiValidationResponse() : base((int)HttpStatusCode.BadRequest)
    {
    }
}
