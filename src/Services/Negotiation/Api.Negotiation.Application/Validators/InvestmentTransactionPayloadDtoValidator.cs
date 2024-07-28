using Api.Negotiation.Application.Dtos.Payloads;
using Api.Negotiation.Domain.Constants;
using FluentValidation;

namespace Api.Negotiation.Application.Validators;

public class InvestmentTransactionPayloadDtoValidator : AbstractValidator<InvestmentTransactionPayloadDto>
{
    public InvestmentTransactionPayloadDtoValidator()
    {
        RuleFor(transaction => transaction.ProductId)
            .NotEmpty().WithMessage("ProductId cannot be empty");

        RuleFor(transaction => transaction.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero");
    }
}