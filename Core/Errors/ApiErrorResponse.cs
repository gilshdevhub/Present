namespace Core.Errors;

public class ApiErrorResponse
{
    public ApiErrorResponse()
    {
    }

    public ApiErrorResponse(int statusCode)
    {
        StatusCode = statusCode;
    }

    public ApiErrorResponse(string message)
    {
        Message = message;
    }

    public ApiErrorResponse(int statusCode, string message) : this(statusCode)
    {
        Message = message ?? GetDefualtMessage(statusCode);
    }

    public ApiErrorResponse(int statusCode, IEnumerable<string> errors) : this(statusCode)
    {
        Errors = errors;
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }
    public IEnumerable<string> Errors { get; set; }

    private string GetDefualtMessage(int statusCode)
    {
        return statusCode switch
        {
            400 => "A bad request you have made",
            401 => "Authorized you are not",
            404 => "resource found, it was not",
            500 => "Error are the path to the dark side. Errors lead to anger. anger leads to hate. hate leads to career change",
            _ => null
        };
    }
}
