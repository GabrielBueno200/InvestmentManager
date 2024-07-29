using Api.FinancialProduct.Application.Mapping;
using Api.FinancialProduct.Application.Products.Queries.GetById;
using Api.FinancialProduct.Domain.Constants;
using Api.FinancialProduct.Domain.Entities;
using Api.FinancialProduct.Domain.Errors;
using Api.FinancialProduct.Domain.Interfaces.Repositories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace FinancialProduct.UnitTests.Application.Products.Queries;

public class GetProductByIdQueryHandlerTest
{
    private readonly IProductRepository _productRepository;
    private readonly GetProductByIdQueryHandler _handler;

    public GetProductByIdQueryHandlerTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new GetProductByIdQueryHandler(_productRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var query = new GetProductByIdQuery("existing-product-id");

        var product = new Product
        {
            Id = query.Id,
            Name = "Name",
            Price = 20,
            Description = "Description",
            MaturityDate = DateTime.Today,
            Type = ProductType.Coins,
            Amount = 10,
            PriceHistory = [100]
        };

        _productRepository.GetByIdAsync(query.Id).Returns(product);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value.Should().BeEquivalentTo(product.ToResponseDto());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProductDoesNotExist()
    {
        // Arrange
        var query = new GetProductByIdQuery("non-existing-product-id");

        _productRepository.GetByIdAsync(query.Id).ReturnsNull();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Error.Should().Be(ProductErrors.NotFound);
    }
}
