using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InvestmentManager.Shared.Utilities.Abstractions;
using Microsoft.AspNetCore.Http;

namespace InvestmentManager.Shared.Configurations.Contexts;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string? GetToken()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            throw new InvalidOperationException("HttpContext is not available");
        }

        var authorizationHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
        
        if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
        {
            return authorizationHeader.Replace("Bearer ", string.Empty);
        }

        return null;
    }

    public SummarizedUser GetLoggedUser()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is not available");
        }

        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var userName = httpContext.User.FindFirst(JwtRegisteredClaimNames.Name)?.Value!;
        
        return new(userId, userName);
    }
}
