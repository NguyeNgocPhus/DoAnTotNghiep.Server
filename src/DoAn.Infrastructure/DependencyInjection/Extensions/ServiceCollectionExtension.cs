using DoAn.Application.Abstractions;
using DoAn.Infrastructure.Authentication;
using DoAn.Infrastructure.Authorization;
using DoAn.Infrastructure.BackgroundJobs;
using DoAn.Infrastructure.Caching.Services;
using DoAn.Infrastructure.Mapper;
using DoAn.Infrastructure.Notification.Services;
using DoAn.Infrastructure.Workflow.Activities.Actions;
using DoAn.Infrastructure.Workflow.Activities.Triggers;
using DoAn.Infrastructure.Workflow.Providers;
using DoAn.Infrastructure.Workflow.Services;
using DotLiquid;
using DotLiquid.Tags;
using Elsa;
using Elsa.Persistence.EntityFramework.Core;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.SqlServer;
using Elsa.Services;
using Elsa.Services.Workflows;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Condition = DoAn.Infrastructure.Workflow.Activities.Actions.Condition;

namespace DoAn.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServicesInfrastructure(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(WorkflowProfile));
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
        return services;
    }
    // Configure Job
    public static void AddQuartzInfrastructure(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(NotificationBackgroundJobs));

            configure
                .AddJob<NotificationBackgroundJobs>(jobKey)
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithInterval(TimeSpan.FromSeconds(5))
                                        .RepeatForever()));

            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService();
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
        services.AddScoped<IWorkflowDefinitionService, WorkflowDefinitionService>();
        services.AddScoped<IWorkflowInstanceService, WorkflowInstanceService>();
        services.AddScoped<IWorkflowLaunchpadService, WorkflowLaunchpadService>();
        
        var connectionString = configuration.GetConnectionString("Workflow");
        var elsaSection = configuration.GetSection("Elsa");
        services.AddElsa(elsa => elsa
                .AddActivity<FileUpload>()
                .AddActivity<Approve>()
                .AddActivity<UpdateStatus>()
                .AddActivity<Reject>()
                .AddActivity<Condition>()
                .AddActivity<Branch>()
                .AddActivity<SendEmail>()
                .AddActivity<Finish>()
                .AddQuartzTemporalActivities()
                .AddHttpActivities(elsaSection.GetSection("Server").Bind)
                .UseEntityFrameworkPersistence(ef => ef.UseSqlServer(connectionString))
                .NoCoreActivities()
                  
            
            // .AddCustomTenantAccessor<CustomTenantAccessor>()
        );
        services.AddBookmarkProvider<UploadFileBookmarkProvider>();
        services.AddBookmarkProvider<ApproveBookmarkProvider>();
        services.AddBookmarkProvider<RejectBookmarkProvider>();
        services.AddScoped<ElsaContext, ElsaContext>();
        services.AddElsaApiEndpoints();
        
        services.AddRazorPages();
        services.AddScoped<IWorkflowRegistry, WorkflowRegistry>();
    }
}