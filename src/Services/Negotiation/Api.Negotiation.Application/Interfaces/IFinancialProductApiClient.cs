using InvestmentManager.Shared.Utilities.Contracts.Dtos;

namespace Api.Negotiation.Application.Interfaces;

public interface IFinancialProductApiClient
{
    Task<ProductResponseDto> GetProductByIdAsync(string productId);
}
