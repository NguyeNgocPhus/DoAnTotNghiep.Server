using DoAn.Application.Abstractions;
using DoAn.Infrastructure.Authentication;
using DoAn.Infrastructure.Caching.Services;
using Elsa;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoAn.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServicesInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICacheService, CacheService>();

        return services;
    }
    public static void AddRedisServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            var connectionString = configuration.GetConnectionString("Redis");
            redisOptions.Configuration = connectionString;
        });
    }
    public static void AddWorkflowInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Workflow");
        var elsaSection = configuration.GetSection("Elsa");
        services.AddElsa(elsa => elsa
                // .AddActivity<WebhookTrigger>()
                // .AddActivity<Filter>()
                // .AddActivity<ChooseAgents>()
                // .AddActivity<HttpResponseActivity>()
                .AddQuartzTemporalActivities()
                .AddHttpActivities(elsaSection.GetSection("Server").Bind)
                .UseEntityFrameworkPersistence(ef => ef.UseSqlServer(connectionString))
                .AddConsoleActivities()
                .AddJavaScriptActivities()
            // .AddCustomTenantAccessor<CustomTenantAccessor>()
        );
        services.AddElsaApiEndpoints();
        services.AddRazorPages();
    }
}