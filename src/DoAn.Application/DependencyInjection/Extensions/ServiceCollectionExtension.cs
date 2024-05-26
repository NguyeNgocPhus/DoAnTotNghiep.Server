using DoAn.Application.Abstractions;
using DoAn.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DoAn.Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServicesApplication(this IServiceCollection services)
    {
        services.AddScoped<LinqExpressionService, LinqExpressionService >();
        services.AddScoped<IPasswordGeneratorService, PasswordGeneratorService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
       
        return services;
    }
    public static IServiceCollection AddMediatRApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly))
            .AddValidatorsFromAssembly(Shared.AssemblyReference.Assembly, includeInternalTypes: true);
        services.AddAutoMapper(AssemblyReference.Assembly);
        return services;
    }
}