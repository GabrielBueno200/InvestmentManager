using Api.FinancialProduct.Application.Mapping;
using Api.FinancialProduct.Application.Products.Commands.Create;
using Api.FinancialProduct.Domain.Constants;
using Api.FinancialProduct.Domain.Entities;
using Api.FinancialProduct.Domain.Interfaces.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using NSubstitute;

namespace Api.FinancialProduct.UnitTests.Application.Products.Commands;

public class CreateProductCommandHandlerTest
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<CreateProductCommand> _validator;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _validator = Substitute.For<IValidator<CreateProductCommand>>();
        _handler = new CreateProductCommandHandler(_productRepository, _validator);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationErrors_WhenValidationFails()
    {
        // Arrange
        var command = new CreateProductCommand("name", "description", DateTime.Now, 0, ProductType.Bond, 0);
        var validationResult = new ValidationResult([new ValidationFailure("Name", "Name is required")]);
        _validator.Validate(command).Returns(validationResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().NotBe(Error.None);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProductIsCreated()
    {
        // Arrange
        var command = new CreateProductCommand("name", "description", DateTime.Now, 5, ProductType.Bond, 100);

        var productToCreate = new Product
        {
            Name = command.Name,
            Price = command.Price,
            Description = command.Description,
            MaturityDate = command.MaturityDate,
            Type = command.Type,
            Amount = command.Amount,
            PriceHistory = [command.Price]
        };

        var createdProduct = new Product
        {
            Id = "id",
            Name = command.Name,
            Price = command.Price,
            Description = command.Description,
            MaturityDate = command.MaturityDate,
            Type = command.Type,
            Amount = command.Amount,
            PriceHistory = productToCreate.PriceHistory
        };

        _validator.Validate(command).Returns(new ValidationResult());
        _productRepository.AddAsync(Arg.Any<Product>()).Returns(createdProduct);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(Error.None);
    }
}
