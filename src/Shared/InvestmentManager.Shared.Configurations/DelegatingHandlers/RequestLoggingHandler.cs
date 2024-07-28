using InvestmentManager.Shared.Configurations.Exceptions;
using InvestmentManager.Shared.Configurations.Extensions;
using Microsoft.Extensions.Logging;

namespace InvestmentManager.Shared.Configurations.DelegatingHandlers;

public class RequestLoggingHandler(ILogger<RequestLoggingHandler> logger) : DelegatingHandler
{
    private readonly ILogger<RequestLoggingHandler> _logger = logger;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(responseContent);
            throw new RequestFailedException(response.StatusCode, responseContent);
        }

        _logger.LogInformation(responseContent);

        return response;
    }
}