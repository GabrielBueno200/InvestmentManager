using Api.FinancialProduct.Application.Products.Queries.GetAll;
using Api.FinancialProduct.Domain.Entities;
using Api.FinancialProduct.Domain.Interfaces.Repositories;
using FluentAssertions;
using InvestmentManager.Shared.Utilities.Abstractions;
using NSubstitute;

namespace FinancialProduct.UnitTests.Application.Products.Queries;

public class GetAllProductsQueryHandlerTest
{
    private readonly IProductRepository _productRepository;
    private readonly GetAllProductsQueryHandler _handler;

    public GetAllProductsQueryHandlerTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new GetAllProductsQueryHandler(_productRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnProducts_WhenProductsExist()
    {
        // Arrange
        var query = new GetAllProductsQuery(10);

        var products = new PaginatedResult<Product>
        {
            Items = [
                new Product { Id = "1", Name = "Product 1", Price = 100 },
                new Product { Id = "2", Name = "Product 2", Price = 150 }
            ],
            TotalCount = 2
        };

        _productRepository.GetAllAsync(query.PageSize, query.LastId).Returns(products);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnNoContent_WhenNoProductsExist()
    {
        // Arrange
        var query = new GetAllProductsQuery(10);

        var products = new PaginatedResult<Product>
        {
            Items = Enumerable.Empty<Product>(),
            TotalCount = 0
        };

        _productRepository.GetAllAsync(query.PageSize, query.LastId).Returns(products);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value?.Items.Should().BeEmpty();
    }
}
