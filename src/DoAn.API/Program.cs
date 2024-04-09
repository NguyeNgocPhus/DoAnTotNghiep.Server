

using DoAn.API.DependencyInjection.Extensions;
using DoAn.API.DependencyInjection.Options;
using DoAn.API.Middlewares;
using DoAn.Application.DependencyInjection.Extensions;
using DoAn.Infrastructure.DependencyInjection.Extensions;
using DoAn.Persistence.Configurations.Configurations;
using DoAn.Persistence.DependencyInjection.Extensions;
using DoAn.Persistence.DependencyInjection.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


#region logger
builder
    .Host
    .UseSerilog(
        (context, _, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
            // To allow to add custom properties into the context
            //configuration.Enrich.FromGlobalLogContext();
        }
    );
builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    //.MinimumLevel.Verbose()
    .CreateLogger();
Log.Logger = logger;
builder.Logging.AddSerilog(logger);

#endregion

// Add Middleware => Remember using middleware
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddJwtAuthenticationAPI(builder.Configuration);
builder.Services.AddMediatRApplication();

builder.Services.AddServicesInfrastructure();
builder.Services.AddRedisServiceInfrastructure(builder.Configuration);
builder.Services.AddWorkflowInfrastructure(builder.Configuration);

// Persistence Layer
//dotnet ef migrations add "Init" --project DoAn.Persistence --context ApplicationDbContext --startup-project DoAn.Api --output-dir Migrations 
// dotnet ef database update --project DoAn.Persistence  --startup-project DoAn.Api --context ApplicationDbContext   
builder.Services.AddSqlServicePersistence();
builder.Services.AddRepositoryPersistence();
builder.Services.ConfigureSqlServerRetryOptionsPersistence(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    .WithExposedHeaders("Content-Disposition")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseCors();
app.UseStaticFiles();
app.UseHttpActivities();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
    endpoints.MapControllers()
        // .RequireAuthorization()
        ;
    endpoints.MapFallbackToPage("/_Host");

});


try
{
    Log.Information("Starting web host");
    app.UseSerilogRequestLogging();
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}