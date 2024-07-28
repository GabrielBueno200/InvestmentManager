using Api.FinancialProduct.Application.Products.Commands.Create;
using Api.FinancialProduct.Application.Products.Commands.Update;
using Api.FinancialProduct.Domain.Entities;
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;

namespace Api.FinancialProduct.Application.Mapping;

public static class ProductMapping
{
    public static Product ToProduct(this CreateProductCommand command) => new()
    {
        Name = command.Name,
        Description = command.Description,
        MaturityDate = command.MaturityDate,
        Price = command.Price,
        Amount = command.Amount,
        Type = command.Type,
        PriceHistory = []
    };

    public static Product ToProduct(this UpdateProductCommand command) => new()
    {
        Id = command.Id,
        Name = command.Name,
        Description = command.Description,
        MaturityDate = command.MaturityDate,
        Price = command.Price,
        Amount = command.Amount,
        Type = command.Type
    };

    public static ProductResponseDto ToResponseDto(this Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        MaturityDate = product.MaturityDate,
        Price = product.Price,
        Amount = product.Amount,
        Type = product.Type,
        PriceHistory = product.PriceHistory,
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt
    };

    public static PaginatedResult<ProductResponseDto> ToResponseDto(
        this PaginatedResult<Product> paginatedResult) => new()
    {
        Items = paginatedResult.Items.Select(product => product.ToResponseDto()),
        TotalCount = paginatedResult.TotalCount,
        PageSize = paginatedResult.PageSize,
        LastId = paginatedResult.LastId
    };
}
