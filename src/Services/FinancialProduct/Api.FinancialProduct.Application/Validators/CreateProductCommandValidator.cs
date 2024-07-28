namespace Api.FinancialProduct.Application.Validators;

using Api.FinancialProduct.Application.Products.Commands.Create;
using Api.FinancialProduct.Domain.Constants;
using FluentValidation;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty().WithMessage("Product name is required")
            .Length(3, 50).WithMessage("Product name must be between 3 and 50 characters");

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