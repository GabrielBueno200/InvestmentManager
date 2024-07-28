using Api.FinancialProduct.Domain.Enums;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;
using MediatR;
using MongoDB.Bson;

namespace Api.FinancialProduct.Application.Products.Commands.Update;

public record UpdateProductCommand(
    string Id,
    string? Name, 
    string? Description, 
    DateTime MaturityDate,
    int Amount,
    ProductType Type,
    decimal Price) : IRequest<Result<ProductResponseDto>>;
