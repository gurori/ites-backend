namespace ites.Core.Exeptions
{
    public class ApiException(string message, int statusCode)
        : Exception(message)
    {
        public int StatusCode { get; } = statusCode;
    }

    public class NotFoundException(string message, int statusCode = 404)
        : ApiException(message, statusCode)
    { }

    public class ConflictException(string message, int statusCode = 409)
        : ApiException(message, statusCode)
    { }

    public class UnauthorizedException(string message = "Проблемы с токеном", int statusCode = 401)
    : ApiException(message, statusCode)
    { }
}
