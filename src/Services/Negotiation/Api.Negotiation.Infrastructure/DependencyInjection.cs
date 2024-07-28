using Api.Negotiation.Application.Interfaces;
using Api.Negotiation.Domain.Interfaces;
using Api.Negotiation.Infrastructure.ApiClients;
using Api.Negotiation.Infrastructure.Repositories;
using Api.Negotiation.Infrastructure.Settings;
using InvestmentManager.Shared.Configurations.DelegatingHandlers;
using InvestmentManager.Shared.Configurations.Extensions;
using InvestmentManager.Shared.Configurations.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Api.Negotiation.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services) =>
        services.AddSettings()
                .AddRepositories()
                .AddApiClients()
                .AddDistributedCache()
                .AddUserContext();

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
         services.AddScoped<IInvestmentTransactionRepository, InvestmentTransactionRepository>();

    private static IServiceCollection AddSettings(this IServiceCollection services) => 
        services.AddBindedSettings<DatabaseSettings>()
                .AddBindedSettings<FinancialProductApiSettings>()
                .AddBindedSettings<JwtSettings>()
                .AddBindedSettings<DistributedCacheSettings>();

    private static IServiceCollection AddApiClients(this IServiceCollection services)
    {
        services.AddTransient<RequestLoggingHandler>();

        services.AddScoped<IFinancialProductApiClient, FinancialProductApiClient>();
        
        services.AddHttpClient<IFinancialProductApiClient, FinancialProductApiClient>((serviceProvicer, client) =>
        {
            var settings = serviceProvicer.GetRequiredService<IOptions<FinancialProductApiSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl);
        })
        .AddHttpMessageHandler<RequestLoggingHandler>();

        return services;
    }
}
