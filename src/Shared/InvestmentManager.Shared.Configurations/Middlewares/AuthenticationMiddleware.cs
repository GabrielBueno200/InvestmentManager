using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace InvestmentManager.Shared.Configurations.Middlewares;

public class AuthenticationMiddleware(
    ILogger<AuthenticationMiddleware> logger,
    TokenValidationParameters tokenValidationParameters) : IMiddleware
{
    private readonly ILogger<AuthenticationMiddleware> _logger = logger;
    private readonly TokenValidationParameters _tokenValidationParameters = tokenValidationParameters;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var token = context.Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Split(" ")
            .Last();

        if (token is null)
        {
            await next(context);
            return;
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

            if (validatedToken != null)
            {
                context.User = principal;
            }

            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError("Token validation failed: {Message}", ex.Message);

            var error = new Error(
                "Invalid Token", 
                "The provided authentication token is invalid",
                HttpStatusCode.Unauthorized
            );
        
            context.Response.StatusCode = error.StatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(error, _serializerOptions));
            return;
        }
    }
}
