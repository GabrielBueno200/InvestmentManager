using Api.FinancialProduct.Application.Mapping;
using Api.FinancialProduct.Application.Products.Queries.GetAll;
using Api.FinancialProduct.Application.Products.Queries.GetById;
using Api.FinancialProduct.Domain.Entities;
using Api.FinancialProduct.Domain.Interfaces.Repositories;
using FluentAssertions;
using InvestmentManager.Shared.Utilities.Abstractions;
using NSubstitute;

namespace FinancialProduct.UnitTests.Application.Products.Queries;

public class GetAvailableProductsQueryHandlerTest
{
    private readonly IProductRepository _productRepository;
    private readonly GetAvailableProductsQueryHandler _handler;

    public GetAvailableProductsQueryHandlerTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new GetAvailableProductsQueryHandler(_productRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnNoContent_WhenNoProductsAvailable()
    {
        // Arrange
        var request = new GetAvailableProductsQuery(10);
        var emptyPaginatedResult = new PaginatedResult<Product>
        {
            Items = []
        };

        _productRepository.GetAvailableProductsAsync(request.PageSize, request.LastId)
            .Returns(emptyPaginatedResult);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Value!.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnProducts_WhenProductsAvailable()
    {
        // Arrange
        var request = new GetAvailableProductsQuery(10);
        var products = new PaginatedResult<Product>
        {
            Items = [
                new Product { Name = "Product1", Price = 10.0m },
                new Product { Name = "Product2", Price = 20.0m }
            ]
        };

        _productRepository.GetAvailableProductsAsync(request.PageSize, request.LastId)
            .Returns(products);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Value!.Items.Should().NotBeEmpty();
    }
}
