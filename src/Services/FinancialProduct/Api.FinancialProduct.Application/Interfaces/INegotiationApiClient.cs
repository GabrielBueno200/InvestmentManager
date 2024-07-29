using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;

namespace Api.FinancialProduct.Application.Interfaces;

public interface INegotiationApiClient
{
    Task<PaginatedResult<InvestmentTransactionResponseDto>> GetTransactionsByProductAsync(
        string productId, int pageSize, string? lastId = null);
}
