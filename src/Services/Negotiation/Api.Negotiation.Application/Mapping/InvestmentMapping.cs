using Api.Negotiation.Domain.Entitites;
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;

namespace Api.Negotiation.Application.Mapping;

public static class InvestmentMapping
{
    public static InvestmentTransactionResponseDto ToResponseDto(
        this InvestmentTransaction investmentTransaction) => new()
    {
        Id = investmentTransaction.Id,
        ProductId  = investmentTransaction.ProductId,
        User = investmentTransaction.User,
        Amount = investmentTransaction.Amount,
        TotalPrice = investmentTransaction.TotalPrice,
        Type = investmentTransaction.Type,
        CreatedAt = investmentTransaction.CreatedAt,
        UpdatedAt = investmentTransaction.UpdatedAt
    };

    public static PaginatedResult<InvestmentTransactionResponseDto> ToResponseDto(
        this PaginatedResult<InvestmentTransaction> paginatedResult) => new()
    {
        Items = paginatedResult.Items.Select(investment => investment.ToResponseDto()),
        TotalCount = paginatedResult.TotalCount,
        PageSize = paginatedResult.PageSize,
        LastId = paginatedResult.LastId
    };
}
