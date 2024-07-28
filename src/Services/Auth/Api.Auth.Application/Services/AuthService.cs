
using FluentValidation;
using Api.Auth.Domain.Entities;
using Api.Auth.Domain.Errors;
using Api.Auth.Application.Interfaces;
using Api.Auth.Application.Dtos.Payloads;
using Api.Auth.Application.Dtos.Responses;
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using InvestmentManager.Shared.Utilities.Constants;
using InvestmentManager.Shared.Utilities.Helpers;

namespace Api.Auth.Application.Services;

public class AuthService(
    IRepository<User> userRepository, 
    IEncryptorService encryptorService,
    ITokenService tokenService,
    IValidator<LoginPayloadDto> loginValidator,
    IValidator<RegisterUserPayloadDto> registerValidator,
    IValidator<AssingRolePayloadDto> assignRoleValidator) : IAuthService
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IEncryptorService _encryptorService = encryptorService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IValidator<LoginPayloadDto> _loginValidator = loginValidator;
    private readonly IValidator<RegisterUserPayloadDto> _registerValidator = registerValidator;
    private readonly IValidator<AssingRolePayloadDto> _assignRoleValidator = assignRoleValidator;

    public async Task<Result<TokenResponseDto>> AuthenticateAsync(LoginPayloadDto payload)
    {
        var validation = _loginValidator.Validate(payload);

        if (!validation.IsValid)
        {
            return validation.GetValidationErrors<TokenResponseDto>();
        }

        var user = (await _userRepository.FilterByAsync(user => user.Email == payload.Email)).FirstOrDefault();

        var areInvalidCredentials = user is null || _encryptorService.Decrypt(user.PasswordHash) != payload.Password;

        if (areInvalidCredentials)
        {
            return Result.Failure<TokenResponseDto>(AuthErrors.InvalidCredentials);
        }

        var (token, expiresIn) = _tokenService.GenerateToken(user!);

        return new TokenResponseDto(token, expiresIn);
    }

    public async Task<Result> RegisterAsync(RegisterUserPayloadDto payload)
    {
        var validation = _registerValidator.Validate(payload);

        if (!validation.IsValid)
        {
            return validation.GetValidationErrors();
        }

        var passwordHash = _encryptorService.Encrypt(payload.Password);

        var user = new User 
        { 
            Username = payload.Username, 
            Email = payload.Email, 
            PasswordHash = passwordHash, 
            Role = Role.Customer 
        };

        await _userRepository.AddAsync(user);

        return Result.Success();
    }

    public async Task<Result> AssignRoleAsync(AssingRolePayloadDto payload)
    {
        var validation = _assignRoleValidator.Validate(payload);

        if (!validation.IsValid)
        {
            return validation.GetValidationErrors();
        }

        var user = (await _userRepository.FilterByAsync(user => user.Email == payload.Email)).FirstOrDefault();

        if (user is null)
        {
            return Result.Failure(AuthErrors.UserNotFound);
        }

        user.Role = payload.Role;
        await _userRepository.UpdateAsync(user);

        return Result.Success();
    }
}