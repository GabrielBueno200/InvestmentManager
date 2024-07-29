using System.Net;
using Api.Negotiation.Application.Dtos.Payloads;
using Api.Negotiation.Application.Interfaces;
using Api.Negotiation.Application.Mapping;
using Api.Negotiation.Domain.Entitites;
using Api.Negotiation.Domain.Errors;
using Api.Negotiation.Domain.Interfaces;
using FluentValidation;
using InvestmentManager.Shared.Configurations.Contexts;
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;
using InvestmentManager.Shared.Utilities.Helpers;

namespace Api.Negotiation.Application.Services;

public class TransactionService(
    IFinancialProductApiClient financialProductApiClient,
    IInvestmentTransactionRepository transactionRepository,
    IValidator<InvestmentTransactionPayloadDto> validator,
    IUserContext userContext) : ITransactionService
{
    private readonly IFinancialProductApiClient _financialProductApiClient = financialProductApiClient;
    private readonly IInvestmentTransactionRepository _transactionRepository = transactionRepository;
    private readonly IValidator<InvestmentTransactionPayloadDto> _validator = validator;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<Result<InvestmentTransactionResponseDto>> CreateTransactionAsync(
        InvestmentTransactionPayloadDto payload, string type)
    {
        var validation = _validator.Validate(payload);

        if (!validation.IsValid)
        {
            return validation.GetValidationErrors<InvestmentTransactionResponseDto>();
        }

        var product = await _financialProductApiClient.GetProductByIdAsync(payload.ProductId);

        var isProductAvailable = product.Amount > 0 && product.MaturityDate >= DateTime.Today;
        
        if (!isProductAvailable)
        {
            return Result.Failure<InvestmentTransactionResponseDto>(InvestmentErrors.UnavailableProduct);
        }

        var transactionToSave = new InvestmentTransaction()
        {
            ProductId = product.Id,
            User = _userContext.GetLoggedUser(),
            Amount = payload.Amount,
            TotalPrice = product.Price * payload.Amount,
            Type = type
        };

        var createdTransaction = await _transactionRepository.AddAsync(transactionToSave);

        return createdTransaction.ToResponseDto()!;
    }

    public async Task<Result<PaginatedResult<InvestmentTransactionResponseDto>>> GetInvestmentsByProductAsync(
        string productId, int pageSize, string? lastId = null)
    {
        var transactions = await _transactionRepository.GetByProductIdAsync(productId, pageSize, lastId);

        var mappedTransactions = transactions.ToResponseDto();

        if (!transactions.Items.Any())
        {
            return Result.Success(mappedTransactions, HttpStatusCode.NoContent);
        }

        return mappedTransactions;
    }

    public async Task<Result<PaginatedResult<InvestmentTransactionResponseDto>>> GetUserInvestmentAsync(
        int pageSize, string? lastId = null)
    {
        var loggedUser = _userContext.GetLoggedUser();

        var transactions = await _transactionRepository.GetUserInvestments(
            loggedUser.Id, pageSize, lastId);

        var mappedTransactions = transactions.ToResponseDto();

        if (!transactions.Items.Any())
        {
            return Result.Success(mappedTransactions, HttpStatusCode.NoContent);
        }

        return mappedTransactions;
    }
}
