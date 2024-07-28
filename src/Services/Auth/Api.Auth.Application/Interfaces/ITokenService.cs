using Api.Auth.Domain.Entities;

namespace Api.Auth.Application.Interfaces;

public interface ITokenService
{
    (string token, int expiresIn) GenerateToken(User user);
}
