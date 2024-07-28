using Api.FinancialProduct.Application.Products.Commands.Update;
using Api.FinancialProduct.Domain.Constants;
using FluentValidation;

namespace Api.FinancialProduct.Application.Validators;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(product => product.Id)
            .NotEmpty().WithMessage("Id is required");

        RuleFor(product => product.Description)
            .NotEmpty().WithMessage("Product description is required")
            .MaximumLength(200).WithMessage("Product description must be 200 characters or fewer");

        RuleFor(product => product.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero");

        RuleFor(product => product.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero");
        
        RuleFor(product => product.Type)
            .NotEmpty().WithMessage("Product type is required")
            .Must(BeAValidType).WithMessage("Invalid type. Valid types are Bond, Stock, Fund, Coins and Commodities");;

        RuleFor(product => product.MaturityDate)
            .NotNull()
            .NotEmpty().WithMessage("Maturity date is required")
            .GreaterThanOrEqualTo(DateTime.Today.Date).WithMessage("Maturity date must be today or in the future")
            .LessThanOrEqualTo(DateTime.Today.Date.AddYears(10)).WithMessage("Maturity date cannot be more than 10 years in the future");
    }

    private bool BeAValidType(int type) => type is
        ProductType.Bond 
        or ProductType.Stock  
        or ProductType.Fund
        or ProductType.Coins
        or ProductType.Commodities;
}