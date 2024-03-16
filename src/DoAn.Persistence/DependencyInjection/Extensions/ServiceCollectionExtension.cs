using DoAn.Persistence.DependencyInjection.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DoAn.Persistence.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSqlServicePersistence(this IServiceCollection services)
    {
        services.AddDbContextPool<DbContext, ApplicationDbContext>((provider, builder) =>
        {
            // Interceptor

            var configuration = provider.GetRequiredService<IConfiguration>();
            var options = provider.GetRequiredService<IOptionsMonitor<SqlServerRetryOptions>>();

            #region ============== SQL-SERVER-STRATEGY-1 ==============

            builder
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true)
                .UseLazyLoadingProxies(
                    true) // => If UseLazyLoadingProxies, all of the navigation fields should be VIRTUAL
                .UseSqlServer(
                    connectionString: configuration.GetConnectionString("ConnectionStrings"),
                    sqlServerOptionsAction: optionsBuilder
                        => optionsBuilder.ExecutionStrategy(
                                dependencies => new SqlServerRetryingExecutionStrategy(
                                    dependencies: dependencies,
                                    maxRetryCount: options.CurrentValue.MaxRetryCount,
                                    maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                                    errorNumbersToAdd: options.CurrentValue.ErrorNumbersToAdd))
                            .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
                ;

            #endregion ============== SQL-SERVER-STRATEGY-1 ==============

            #region ============== SQL-SERVER-STRATEGY-2 ==============

            //builder
            //.EnableDetailedErrors(true)
            //.EnableSensitiveDataLogging(true)
            //.UseLazyLoadingProxies(true) // => If UseLazyLoadingProxies, all of the navigation fields should be VIRTUAL
            //.UseSqlServer(
            //    connectionString: configuration.GetConnectionString("ConnectionStrings"),
            //        sqlServerOptionsAction: optionsBuilder
            //            => optionsBuilder
            //            .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));

            #endregion ============== SQL-SERVER-STRATEGY-2 ==============



        });
        return services;
    }
    
}