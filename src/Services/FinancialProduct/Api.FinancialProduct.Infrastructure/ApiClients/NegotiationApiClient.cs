using Api.FinancialProduct.Application.Interfaces;
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;

namespace Api.FinancialProduct.Infrastructure.ApiClients;

public class NegotiationApiClient : INegotiationApiClient
{
    public Task<PaginatedResult<InvestmentTransactionResponseDto>> GetTransactionsByProductAsync(
        string productId, int pageSize, string? lastId = null)
    {
        throw new NotImplementedException();
    }
}
