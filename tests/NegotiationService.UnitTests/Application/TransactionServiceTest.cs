using Api.Negotiation.Application.Dtos.Payloads;
using Api.Negotiation.Application.Interfaces;
using Api.Negotiation.Application.Mapping;
using Api.Negotiation.Application.Services;
using Api.Negotiation.Domain.Entitites;
using Api.Negotiation.Domain.Errors;
using Api.Negotiation.Domain.Interfaces;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using InvestmentManager.Shared.Configurations.Contexts;
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;
using NSubstitute;

namespace NegotiationService.UnitTests.Application;

public class TransactionServiceTest
{
    private readonly IFinancialProductApiClient _financialProductApiClient;
    private readonly IInvestmentTransactionRepository _transactionRepository;
    private readonly IValidator<InvestmentTransactionPayloadDto> _validator;
    private readonly IUserContext _userContext;
    private readonly TransactionService _service;

    public TransactionServiceTest()
    {
        _financialProductApiClient = Substitute.For<IFinancialProductApiClient>();
        _transactionRepository = Substitute.For<IInvestmentTransactionRepository>();
        _validator = Substitute.For<IValidator<InvestmentTransactionPayloadDto>>();
        _userContext = Substitute.For<IUserContext>();

        _service = new TransactionService(
            _financialProductApiClient,
            _transactionRepository,
            _validator,
            _userContext
        );
    }

    [Fact]
    public async Task CreateTransactionAsync_ShouldReturnValidationErrors_WhenPayloadIsInvalid()
    {
        // Arrange
        var invalidPayload = new InvestmentTransactionPayloadDto("", -1);
        var validationResult = new ValidationResult([new ValidationFailure("ProductId", "ProductId is required")]);

        _validator.Validate(invalidPayload).Returns(validationResult);

        // Act
        var result = await _service.CreateTransactionAsync(invalidPayload, "Buy");

        // Assert
        result.Error.Should().NotBe(Error.None);
    }

    [Fact]
    public async Task CreateTransactionAsync_ShouldReturnFailure_WhenProductIsUnavailable()
    {
        // Arrange
        var validPayload = new InvestmentTransactionPayloadDto("productId", 5);
        var validationResult = new ValidationResult();

        _validator.Validate(validPayload).Returns(validationResult);

        var unavailableProduct = new ProductResponseDto
        {
            Id = "productId",
            Amount = 0,
            MaturityDate = DateTime.Today.AddDays(-1)
        };
        _financialProductApiClient.GetProductByIdAsync(validPayload.ProductId).Returns(unavailableProduct);

        // Act
        var result = await _service.CreateTransactionAsync(validPayload, "Buy");

        // Assert
        result.Error.Should().Be(InvestmentErrors.UnavailableProduct);
    }

    [Fact]
    public async Task CreateTransactionAsync_ShouldReturnCreatedTransaction_WhenSuccessful()
    {
        // Arrange
        var validPayload = new InvestmentTransactionPayloadDto("productId", 5);
        var validationResult = new ValidationResult();

        _validator.Validate(validPayload).Returns(validationResult);

        var product = new ProductResponseDto
        {
            Id = "productId",
            Amount = 10,
            MaturityDate = DateTime.Today.AddDays(1),
            Price = 100m
        };
        _financialProductApiClient.GetProductByIdAsync(validPayload.ProductId).Returns(product);

        var loggedUser = new SummarizedUser("userId", "Username");
        _userContext.GetLoggedUser().Returns(loggedUser);

        var transaction = new InvestmentTransaction
        {
            ProductId = product.Id,
            User = loggedUser,
            Amount = validPayload.Amount,
            TotalPrice = product.Price * validPayload.Amount,
            Type = "Buy"
        };

        _transactionRepository.AddAsync(Arg.Any<InvestmentTransaction>()).Returns(transaction);

        // Act
        var result = await _service.CreateTransactionAsync(validPayload, "Buy");

        // Assert
        result.Value.Should().BeEquivalentTo(transaction.ToResponseDto());
    }

    [Fact]
    public async Task GetInvestmentsByProductAsync_ShouldReturnNoContent_WhenNoTransactions()
    {
        // Arrange
        var productId = "productId";
        var pageSize = 10;
        var lastId = (string?)null;

        var transactions = new PaginatedResult<InvestmentTransaction>
        {
            Items = new List<InvestmentTransaction>()
        };

        _transactionRepository.GetByProductIdAsync(productId, pageSize, lastId).Returns(transactions);

        // Act
        var result = await _service.GetInvestmentsByProductAsync(productId, pageSize, lastId);

        // Assert
        result.Value!.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task GetInvestmentsByProductAsync_ShouldReturnTransactions_WhenAvailable()
    {
        // Arrange
        var productId = "productId";
        var pageSize = 10;
        var lastId = (string?)null;

        var transactions = new PaginatedResult<InvestmentTransaction>
        {
            Items = 
            [
                new InvestmentTransaction { Id = "transaction1" },
                new InvestmentTransaction { Id = "transaction2" }
            ]
        };

        _transactionRepository.GetByProductIdAsync(productId, pageSize, lastId).Returns(transactions);

        // Act
        var result = await _service.GetInvestmentsByProductAsync(productId, pageSize, lastId);

        // Assert
        result.Value.Should().BeEquivalentTo(transactions.ToResponseDto());
    }

    [Fact]
    public async Task GetUserInvestmentAsync_ShouldReturnNoContent_WhenNoUserTransactions()
    {
        // Arrange
        var pageSize = 10;
        var lastId = (string?)null;

        var loggedUser = new SummarizedUser ("userId", "username");
        _userContext.GetLoggedUser().Returns(loggedUser);

        var transactions = new PaginatedResult<InvestmentTransaction>
        {
            Items = new List<InvestmentTransaction>()
        };

        _transactionRepository.GetUserInvestments(loggedUser.Id, pageSize, lastId).Returns(transactions);

        // Act
        var result = await _service.GetUserInvestmentAsync(pageSize, lastId);

        // Assert
        result.Value!.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task GetUserInvestmentAsync_ShouldReturnTransactions_WhenAvailable()
    {
        // Arrange
        var pageSize = 10;
        var lastId = (string?)null;

        var loggedUser = new SummarizedUser ("userId", "username");
        _userContext.GetLoggedUser().Returns(loggedUser);

        var transactions = new PaginatedResult<InvestmentTransaction>
        {
            Items = 
            [
                new InvestmentTransaction { Id = "transaction1" },
                new InvestmentTransaction { Id = "transaction2" }
            ]
        };

        _transactionRepository.GetUserInvestments(loggedUser.Id, pageSize, lastId).Returns(transactions);

        // Act
        var result = await _service.GetUserInvestmentAsync(pageSize, lastId);

        // Assert
        result.Value.Should().BeEquivalentTo(transactions.ToResponseDto());
    }
}
