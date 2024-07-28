using InvestmentManager.Shared.Configurations.Contexts;
using InvestmentManager.Shared.Configurations.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace InvestmentManager.Shared.Configurations.Extensions;

public static partial class DependencyInjectionExtensions
{
    public static IServiceCollection AddBindedSettings<TSettings>(
        this IServiceCollection services) where TSettings : class
    {
        services.AddOptions<TSettings>()
            .BindConfiguration(typeof(TSettings).Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    public static IServiceCollection AddDistributedCache(this IServiceCollection services)
    {
        var settings = services.BuildServiceProvider()
            .GetRequiredService<IOptions<DistributedCacheSettings>>()?
            .Value!;
            
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(settings.ConnectionString));

        return services;
    }

    public static IServiceCollection AddUserContext(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        
        return services.AddScoped<IUserContext, UserContext>();
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        var settings = services.BuildServiceProvider()
            .GetRequiredService<IOptions<JwtSettings>>()?
            .Value!;

        var key = Convert.FromBase64String(settings.SecretKey);

        TokenValidationParameters validationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidIssuer = settings.Issuer,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        services.AddSingleton(validationParameters);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = validationParameters;
        });

        return services;
    }
}