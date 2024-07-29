using Api.FinancialProduct.Application.Dtos;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using MediatR;

namespace Api.FinancialProduct.Application.Products.Queries.GetExtract;

public record GetProductExtractQuery(string ProductId, int PageSize, string? LastId = null) 
    : IRequest<Result<ProductExtractResponseDto>>;
