using Api.FinancialProduct.Application.Products.Commands.Delete;
using Api.FinancialProduct.Domain.Entities;
using Api.FinancialProduct.Domain.Errors;
using Api.FinancialProduct.Domain.Interfaces.Repositories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace FinancialProduct.UnitTests.Application.Products.Commands;

public class DeleteProductCommandHandlerTest
{
    private readonly IProductRepository _productRepository;
    private readonly DeleteProductCommandHandler _handler;

    public DeleteProductCommandHandlerTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new DeleteProductCommandHandler(_productRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProductDoesNotExist()
    {
        // Arrange
        var command = new DeleteProductCommand("non-existing-id");
        _productRepository.GetByIdAsync(command.Id).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(ProductErrors.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProductIsDeleted()
    {
        // Arrange
        var command = new DeleteProductCommand("existing-id");
        var product = new Product();
        _productRepository.GetByIdAsync(command.Id).Returns(product);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        await _productRepository.Received().DeleteByIdAsync(command.Id);
    }
}
