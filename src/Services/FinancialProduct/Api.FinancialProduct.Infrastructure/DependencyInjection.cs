using Api.FinancialProduct.Domain.Interfaces.Repositories;
using Api.FinancialProduct.Infrastructure.Repositories;
using InvestmentManager.Shared.Configurations.Extensions;
using InvestmentManager.Shared.Configurations.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Api.FinancialProduct.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services) =>
        services.AddSettings()
                .AddRepositories()
                .AddDistributedCache();

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
         services.AddScoped<IProductRepository, ProductRepository>();

    private static IServiceCollection AddSettings(this IServiceCollection services) => 
        services.AddBindedSettings<DatabaseSettings>()
                .AddBindedSettings<DistributedCacheSettings>();
}
