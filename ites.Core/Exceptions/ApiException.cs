namespace ites.Core.Exceptions;

public class ApiException(string message, int statusCode) : ApplicationException(message)
{
    public int StatusCode { get; } = statusCode;
}

public sealed class NotFoundException(string message, int statusCode = 404)
    : ApiException(message, statusCode) { }

public sealed class ConflictException(string message, int statusCode = 409)
    : ApiException(message, statusCode) { }

public sealed class UnauthorizedException(
    string message = "Проблемы с токеном",
    int statusCode = 401
) : ApiException(message, statusCode) { }

public sealed class ForbiddenException(string message, int statusCode = 403)
    : ApiException(message, statusCode) { }

public sealed class BadRequestException(string message, int statusCode = 400)
    : ApiException(message, statusCode) { }
