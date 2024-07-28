using MediatR;
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;

namespace Api.FinancialProduct.Application.Products.Queries.GetAll;

public record GetAllProductsQuery(int PageSize, string? LastId = null) : IRequest<Result<PaginatedResult<ProductResponseDto>>>;