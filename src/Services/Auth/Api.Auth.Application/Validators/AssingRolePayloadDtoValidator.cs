using Api.Auth.Application.Dtos.Payloads;
using FluentValidation;
using InvestmentManager.Shared.Utilities.Constants;

namespace Api.Auth.Application.Validators;

public class AssingRolePayloadDtoValidator : AbstractValidator<AssingRolePayloadDto>
{
    public AssingRolePayloadDtoValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(dto => dto.Role)
            .NotEmpty().WithMessage("Role is required")
            .Must(BeAValidRole).WithMessage("Invalid role. Available roles are Admin, Operation, and Customer");
    }

    private bool BeAValidRole(string role) => role is Role.Admin or Role.Operation or Role.Customer;
}