using FluentAssertions;
using NSubstitute;
using Api.Auth.Application.Dtos.Payloads;
using Api.Auth.Application.Dtos.Responses;
using Api.Auth.Application.Services;
using Api.Auth.Domain.Entities;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using InvestmentManager.Shared.Utilities.Abstractions;
using Api.Auth.Application.Interfaces;
using FluentValidation;
using System.Linq.Expressions;
using InvestmentManager.Shared.Utilities.Constants;
using FluentValidation.Results;

namespace Api.Auth.UnitTests.Services;

public class AuthServiceTest
{
    private readonly IRepository<User> _userRepository;
    private readonly IEncryptorService _encryptorService;
    private readonly ITokenService _tokenService;
    private readonly IValidator<LoginPayloadDto> _loginValidator;
    private readonly IValidator<RegisterUserPayloadDto> _registerValidator;
    private readonly IValidator<AssingRolePayloadDto> _assignRoleValidator;
    private readonly AuthService _authService;

    public AuthServiceTest()
    {
        _userRepository = Substitute.For<IRepository<Domain.Entities.User>>();
        _encryptorService = Substitute.For<IEncryptorService>();
        _tokenService = Substitute.For<ITokenService>();
        _loginValidator = Substitute.For<IValidator<LoginPayloadDto>>();
        _registerValidator = Substitute.For<IValidator<RegisterUserPayloadDto>>();
        _assignRoleValidator = Substitute.For<IValidator<AssingRolePayloadDto>>();

        _authService = new AuthService(
            _userRepository,
            _encryptorService,
            _tokenService,
            _loginValidator,
            _registerValidator,
            _assignRoleValidator);
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var payload = new LoginPayloadDto("test@example.com", "password");
        var validationResult = new ValidationResult([new ValidationFailure("Email", "Invalid email")]);
        _loginValidator.Validate(payload).Returns(validationResult);

        // Act
        var result = await _authService.AuthenticateAsync(payload);

        // Assert
        result.Error.Should().NotBe(Error.None);
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var payload = new LoginPayloadDto("test@example.com", "password");
        var user = new User { Email = "test@example.com", PasswordHash = "hashedPassword" };
        _userRepository.FilterByAsync(Arg.Any<Expression<Func<User, bool>>>()).Returns(new[] { user }.AsQueryable());
        _encryptorService.Decrypt("hashedPassword").Returns("password");
        _tokenService.GenerateToken(user).Returns(("token", 3600));

        _loginValidator.Validate(payload).Returns(new ValidationResult());

        // Act
        var result = await _authService.AuthenticateAsync(payload);

        // Assert
        result.Value.Should().BeEquivalentTo(new TokenResponseDto("token", 3600));
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnSuccess_WhenUserIsRegistered()
    {
        // Arrange
        var payload = new RegisterUserPayloadDto("user", "test@example.com", "password");
        _registerValidator.Validate(payload).Returns(new ValidationResult());
        _userRepository.FilterByAsync(Arg.Any<Expression<Func<User, bool>>>()).Returns(Enumerable.Empty<User>().AsQueryable());
        _encryptorService.Encrypt("password").Returns("hashedPassword");

        // Act
        var result = await _authService.RegisterAsync(payload);

        // Assert
        await _userRepository.Received().AddAsync(Arg.Is<User>(u => u.Email == "test@example.com" && u.PasswordHash == "hashedPassword"));
    }

    [Fact]
    public async Task AssignRoleAsync_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var payload = new AssingRolePayloadDto("test@example.com", Role.Admin);
        var validationResult = new ValidationResult([new ValidationFailure("Email", "Invalid email")]);
        _assignRoleValidator.Validate(payload).Returns(validationResult);

        // Act
        var result = await _authService.AssignRoleAsync(payload);

        // Assert
        result.Error.Should().NotBe(Error.None);
    }

    [Fact]
    public async Task AssignRoleAsync_ShouldReturnSuccess_WhenRoleIsAssigned()
    {
        // Arrange
        var payload = new AssingRolePayloadDto("test@example.com", Role.Admin);
        var user = new User { Email = "test@example.com", Role = Role.Customer };
        _assignRoleValidator.Validate(payload).Returns(new ValidationResult());
        _userRepository.FilterByAsync(Arg.Any<Expression<Func<User, bool>>>()).Returns(new[] { user }.AsQueryable());

        // Act
        var result = await _authService.AssignRoleAsync(payload);

        // Assert
        user.Role.Should().Be(Role.Admin);
        await _userRepository.Received().UpdateAsync(user);
    }
}