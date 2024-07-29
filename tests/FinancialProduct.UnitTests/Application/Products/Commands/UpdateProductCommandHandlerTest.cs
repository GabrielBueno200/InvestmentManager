using Api.FinancialProduct.Application.Mapping;
using Api.FinancialProduct.Application.Products.Commands.Update;
using Api.FinancialProduct.Domain.Entities;
using Api.FinancialProduct.Domain.Errors;
using Api.FinancialProduct.Domain.Interfaces.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace FinancialProduct.UnitTests.Application.Products.Commands;

public class UpdateProductCommandHandlerTest
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<UpdateProductCommand> _validator;
    private readonly UpdateProductCommandHandler _handler;

    public UpdateProductCommandHandlerTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _validator = Substitute.For<IValidator<UpdateProductCommand>>();
        _handler = new UpdateProductCommandHandler(_productRepository, _validator);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationErrors_WhenValidationFails()
    {
        // Arrange
        var command = new UpdateProductCommand(
            Id: "product-id",
            Name: null,
            Description: null,
            MaturityDate: DateTime.Now,
            Amount: 0,
            Type: 1,
            Price: 100m);

        var validationResult = new ValidationResult([new ValidationFailure("Price", "Price is required")]);
        _validator.Validate(command).Returns(validationResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().NotBe(Error.None); // Adjust based on your validation error handling
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProductDoesNotExist()
    {
        // Arrange
        var command = new UpdateProductCommand(
            Id: "non-existing-id",
            Name: "Updated Name",
            Description: "Updated Description",
            MaturityDate: DateTime.Now,
            Amount: 10,
            Type: 1,
            Price: 100m);

        _validator.Validate(command).Returns(new ValidationResult());
        _productRepository.GetByIdAsync(command.Id).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProductIsUpdated()
    {
        // Arrange
        var command = new UpdateProductCommand(
            Id: "existing-id",
            Name: "Updated Name",
            Description: "Updated Description",
            MaturityDate: DateTime.Now,
            Amount: 10,
            Type: 1,
            Price: 120m);

        var existingProduct = new Product
        {
            Id = command.Id,
            Name = "Old Name",
            Description = "Old Description",
            Price = 100m,
            PriceHistory = [100m]
        };

        var updatedProduct = new Product
        {
            Id = command.Id,
            Name = command.Name,
            Description = command.Description,
            Type = command.Type,
            MaturityDate = command.MaturityDate,
            Amount = command.Amount,
            Price = command.Price,
            PriceHistory = [100m, 120m]
        };

        _validator.Validate(command).Returns(new ValidationResult());
        _productRepository.GetByIdAsync(command.Id).Returns(existingProduct);
        _productRepository.UpdateAsync(updatedProduct).Returns(Task.FromResult(updatedProduct));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Value.Should().BeEquivalentTo(updatedProduct.ToResponseDto());
    }
}
