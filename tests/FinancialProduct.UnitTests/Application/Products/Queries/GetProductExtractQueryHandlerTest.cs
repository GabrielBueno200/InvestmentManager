using System.Text.Json;
using Api.FinancialProduct.Application.Dtos;
using Api.FinancialProduct.Application.Interfaces;
using Api.FinancialProduct.Application.Mapping;
using Api.FinancialProduct.Application.Products.Queries.GetExtract;
using Api.FinancialProduct.Domain.Entities;
using Api.FinancialProduct.Domain.Errors;
using Api.FinancialProduct.Domain.Interfaces.Repositories;
using FluentAssertions;
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using StackExchange.Redis;

namespace FinancialProduct.UnitTests.Application.Products.Queries;

public class GetProductExtractQueryHandlerTest
{
    private readonly IProductRepository _productRepository;
    private readonly INegotiationApiClient _negotiationApiClient;
    private readonly IDatabase _database;
    private readonly GetProductExtractQueryHandler _handler;

    public GetProductExtractQueryHandlerTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _negotiationApiClient = Substitute.For<INegotiationApiClient>();
        var muxer = Substitute.For<IConnectionMultiplexer>();
        _database = Substitute.For<IDatabase>();
        muxer.GetDatabase().Returns(_database);

        _handler = new GetProductExtractQueryHandler(_productRepository, _negotiationApiClient, muxer);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenDataIsInCache()
    {
        // Arrange
        var query = new GetProductExtractQuery("productId", 10);
        var cachedData = new ProductExtractResponseDto
        {
            Product = new ProductResponseDto { Name = "Cached Product" },
            Transactions = new PaginatedResult<InvestmentTransactionResponseDto>()
        };

        var cacheKey = $"products-extract-{query.ProductId}{query.PageSize}{query.LastId ?? string.Empty}";
        _database.StringGetAsync(cacheKey).Returns(JsonSerializer.Serialize(cachedData));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value.Should().BeEquivalentTo(cachedData);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var query = new GetProductExtractQuery("productId", 10);
        _productRepository.GetByIdAsync(query.ProductId).ReturnsNull();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Error.Should().Be(ProductErrors.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnProductAndTransactions_WhenProductExists()
    {
        // Arrange
        var query = new GetProductExtractQuery("productId", 10);
        var product = new Product
        {
            Name = "Product Name",
            Price = 100.0m
        };

        var transactions = new PaginatedResult<InvestmentTransactionResponseDto>
        {
            Items = [new InvestmentTransactionResponseDto { Amount = 10 }]
        };

        var extractResponse = new ProductExtractResponseDto
        {
            Product = product.ToResponseDto(),
            Transactions = transactions
        };

        _productRepository.GetByIdAsync(query.ProductId).Returns(product);
        _negotiationApiClient.GetTransactionsByProductAsync(query.ProductId, query.PageSize, query.LastId)
            .Returns(transactions);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value!.Product.Should().NotBeNull();
        result.Value!.Transactions.Items.Should().NotBeEmpty();
    }
}
