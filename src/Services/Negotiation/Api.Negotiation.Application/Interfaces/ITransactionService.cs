using Api.Negotiation.Application.Dtos.Payloads;
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;

namespace Api.Negotiation.Application.Interfaces;

public interface ITransactionService
{
    Task<Result<InvestmentTransactionResponseDto>> CreateTransactionAsync(
        InvestmentTransactionPayloadDto payload, string type);
    Task<Result<PaginatedResult<InvestmentTransactionResponseDto>>> GetInvestmentsByProductAsync(
        string productId, int pageSize, string? lastId = null);
}
