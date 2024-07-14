namespace ites.Core.Exeptions
{
    public class ApiException(string message, int statusCode)
        : Exception(message)
    {
        public int StatusCode { get; } = statusCode;
    }
}
