using System.Reflection;
using Api.Negotiation.Application.Interfaces;
using Api.Negotiation.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Negotiation.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services.AddServices()
                .AddValidators();

    private static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddScoped<ITransactionService, TransactionService>();

    private static IServiceCollection AddValidators(this IServiceCollection services) =>
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
}
