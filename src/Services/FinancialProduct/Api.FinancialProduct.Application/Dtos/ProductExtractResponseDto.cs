using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;

namespace Api.FinancialProduct.Application.Dtos;

public class ProductExtractResponseDto
{
    public ProductResponseDto Product { get; set; }
    public Result<PaginatedResult<InvestmentTransactionResponseDto>> Transactions { get; set; }
}
