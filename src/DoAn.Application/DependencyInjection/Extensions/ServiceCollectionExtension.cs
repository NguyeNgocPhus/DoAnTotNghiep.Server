using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DoAn.Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMediatRApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly))
            .AddValidatorsFromAssembly(Shared.AssemblyReference.Assembly, includeInternalTypes: true);
        return services;
    }
}