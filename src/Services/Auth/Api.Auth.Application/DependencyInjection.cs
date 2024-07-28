using System.Reflection;
using Api.Auth.Application.Interfaces;
using Api.Auth.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services.AddServices()
                .AddValidators();

    private static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddScoped<IAuthService, AuthService>();

    private static IServiceCollection AddValidators(this IServiceCollection services) =>
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
}
