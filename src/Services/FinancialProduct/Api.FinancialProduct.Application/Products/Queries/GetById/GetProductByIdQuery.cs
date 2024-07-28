using InvestmentManager.Shared.Utilities.Abstractions.Result;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;
using MediatR;

namespace Api.FinancialProduct.Application.Products.Queries.GetById;

public record GetProductByIdQuery(string Id) : IRequest<Result<ProductResponseDto>>;