using InvestmentManager.Shared.Utilities.Abstractions.Result;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;
using MediatR;

namespace Api.FinancialProduct.Application.Products.Commands.Update;

public record UpdateProductCommand(
    string Id,
    string? Name, 
    string? Description, 
    DateTime MaturityDate,
    int Amount,
    int Type,
    decimal Price) : IRequest<Result<ProductResponseDto>>;
