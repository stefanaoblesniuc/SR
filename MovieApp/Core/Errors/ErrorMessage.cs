using System.Net;
using System.Text.Json.Serialization;

namespace MovieApp.Core.Errors;

public class ErrorMessage
{
    public string Message { get; }
    public ErrorCodes Code { get; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public HttpStatusCode Status { get; }

    public ErrorMessage(HttpStatusCode status, string message, ErrorCodes code = ErrorCodes.Unknown)
    {
        Message = message;
        Status = status;
        Code = code;
    }

    public static ErrorMessage FromException(ServerException exception) => new(exception.Status, exception.Message);
}
