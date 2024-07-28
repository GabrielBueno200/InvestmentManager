using MediatR;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;
using Api.FinancialProduct.Domain.Enums;

namespace Api.FinancialProduct.Application.Products.Commands.Create;

public record CreateProductCommand(
    string? Name, 
    string? Description, 
    DateTime MaturityDate, 
    int Amount,
    ProductType Type,
    decimal Price) : IRequest<Result<ProductResponseDto>>;
