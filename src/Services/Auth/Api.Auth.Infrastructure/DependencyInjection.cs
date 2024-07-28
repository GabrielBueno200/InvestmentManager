using Api.Auth.Application.Interfaces;
using Api.Auth.Domain.Entities;
using Api.Auth.Infrastructure.Services;
using Api.Auth.Infrastructure.Settings;
using InvestmentManager.Shared.Configurations.Extensions;
using InvestmentManager.Shared.Configurations.Settings;
using InvestmentManager.Shared.Utilities.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Auth.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services) =>
        services.AddSettings()
                .AddServices()
                .AddRepositories();

    private static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddScoped<IEncryptorService, AesEncryptorService>()
                .AddScoped<ITokenService, JwtTokenService>();

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
         services.AddScoped<IRepository<Domain.Entities.User>, Repository<Domain.Entities.User>>();

    private static IServiceCollection AddSettings(this IServiceCollection services) => 
        services.AddBindedSettings<DatabaseSettings>()
                .AddBindedSettings<JwtSettings>()
                .AddBindedSettings<AesSettings>();
}
