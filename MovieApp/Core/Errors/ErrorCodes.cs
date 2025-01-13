using System.Text.Json.Serialization;

namespace MovieApp.Core.Errors;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorCodes
{
    Unknown,
    TechnicalError,
    EntityNotFound,
    PhysicalFileNotFound,
    UserAlreadyExists,
    WrongPassword,
    CannotAdd,
    CannotUpdate,
    CannotDelete,
    MailSendFailed,
    AuthorAlreadyExists,
    BookAlreadyExists,
    GenreAlreadyExists,
    ReviewAlreadyExists
}
