using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Api.Auth.Application.Interfaces;
using Api.Auth.Domain.Entities;
using InvestmentManager.Shared.Configurations.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Auth.Infrastructure.Services;

public class JwtTokenService(IOptions<JwtSettings> jwtSettings) : ITokenService
{
    public readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public (string token, int expiresIn) GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var key = Convert.FromBase64String(_jwtSettings.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Name, user.Username),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Role, user.Role)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            Issuer = _jwtSettings.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        
        var token = tokenHandler.WriteToken(securityToken);

        return (token, _jwtSettings.ExpirationInMinutes);
    }
}
