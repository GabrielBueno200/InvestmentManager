using Api.Negotiation.Application.Interfaces;
using InvestmentManager.Shared.Utilities.Abstractions.Builders;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;

namespace Api.Negotiation.Infrastructure.ApiClients;

public class FinancialProductApiClient(HttpClient httpClient) : IFinancialProductApiClient
{
    private readonly HttpClient _client = httpClient;

    public async Task<ProductResponseDto> GetProductByIdAsync(string productId)
    {
        var request = RequestBuilder
            .Get
            .WithUrl("products")
            .WithPathParam(productId);

        return await request.SendAsync<ProductResponseDto>(_client);
    }
}
