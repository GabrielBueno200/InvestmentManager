using Api.Auth.Application.Dtos.Payloads;
using Api.Auth.Application.Dtos.Responses;
using InvestmentManager.Shared.Utilities.Abstractions.Result;

namespace Api.Auth.Application.Interfaces;

public interface IAuthService
{
    Task<Result<TokenResponseDto>> AuthenticateAsync(LoginPayloadDto payload);
    Task<Result> RegisterAsync(RegisterUserPayloadDto payload);
    Task<Result> AssignRoleAsync(AssingRolePayloadDto payload);
}
