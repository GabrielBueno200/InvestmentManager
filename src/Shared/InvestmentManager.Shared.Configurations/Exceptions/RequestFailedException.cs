using System.Net;

namespace InvestmentManager.Shared.Configurations.Exceptions;

public class RequestFailedException(HttpStatusCode statusCode, string message) : Exception(message)
{
    public HttpStatusCode StatusCode = statusCode;
}
