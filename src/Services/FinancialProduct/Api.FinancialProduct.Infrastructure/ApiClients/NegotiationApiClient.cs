using Api.FinancialProduct.Application.Interfaces;
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Abstractions.Builders;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;

namespace Api.FinancialProduct.Infrastructure.ApiClients;

public class NegotiationApiClient(HttpClient client) : INegotiationApiClient
{
    private readonly HttpClient _client = client;

    public Task<PaginatedResult<InvestmentTransactionResponseDto>> GetTransactionsByProductAsync(
        string productId, int pageSize, string? lastId = null)
    {
        return RequestBuilder
            .Get
            .WithUrl("negotiation/product")
            .WithQueryParam("productId", productId)
            .WithQueryParam("pageSize", pageSize)
            .WithQueryParam("lastId", lastId)
            .SendAsync<PaginatedResult<InvestmentTransactionResponseDto>>(_client);
    }
}
