using Api.FinancialProduct.Application.Interfaces;
using Api.FinancialProduct.Domain.Interfaces.Repositories;
using Api.FinancialProduct.Infrastructure.ApiClients;
using Api.FinancialProduct.Infrastructure.Repositories;
using Api.FinancialProduct.Infrastructure.Settings;
using InvestmentManager.Shared.Configurations.DelegatingHandlers;
using InvestmentManager.Shared.Configurations.Extensions;
using InvestmentManager.Shared.Configurations.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Api.FinancialProduct.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services) =>
        services.AddSettings()
                .AddRepositories()
                .AddApiClients()
                .AddDistributedCache()
                .AddUserContext();

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
         services.AddScoped<IProductRepository, ProductRepository>()
                 .Decorate<IProductRepository, CachedProductRepository>();

    private static IServiceCollection AddSettings(this IServiceCollection services) => 
        services.AddBindedSettings<DatabaseSettings>()
                .AddBindedSettings<NegotiationApiSettings>()
                .AddBindedSettings<JwtSettings>()
                .AddBindedSettings<DistributedCacheSettings>();

    private static IServiceCollection AddApiClients(this IServiceCollection services)
    {
        services.AddTransient<RequestLoggingHandler>();

        services.AddScoped<INegotiationApiClient, NegotiationApiClient>();
        
        services.AddHttpClient<INegotiationApiClient, NegotiationApiClient>((serviceProvicer, client) =>
        {
            var settings = serviceProvicer.GetRequiredService<IOptions<NegotiationApiSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl);
        })
        .AddHttpMessageHandler<RequestLoggingHandler>();

        return services;
    }
}
