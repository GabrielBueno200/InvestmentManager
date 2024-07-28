using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Api.FinancialProduct.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services.AddMediatr()
                .AddValidators();

    private static IServiceCollection AddMediatr(this IServiceCollection services) =>
        services.AddMediatR(configuration => configuration
            .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

    private static IServiceCollection AddValidators(this IServiceCollection services) =>
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
}
