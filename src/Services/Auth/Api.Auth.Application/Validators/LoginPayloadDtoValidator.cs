using Api.Auth.Application.Dtos.Payloads;
using FluentValidation;

namespace Api.Auth.Application.Validators;

public class LoginPayloadDtoValidator : AbstractValidator<LoginPayloadDto>
{
    public LoginPayloadDtoValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(dto => dto.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}