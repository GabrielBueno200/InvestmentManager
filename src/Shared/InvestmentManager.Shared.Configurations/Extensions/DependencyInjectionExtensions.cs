using InvestmentManager.Shared.Configurations.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace InvestmentManager.Shared.Configurations.Extensions;

public static class DependencyInjectionExtensions
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
}