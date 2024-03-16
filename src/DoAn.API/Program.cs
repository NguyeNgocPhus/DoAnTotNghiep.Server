

using DoAn.API.DependencyInjection.Extensions;
using DoAn.Application.DependencyInjection.Extensions;
using DoAn.Infrastructure.DependencyInjection.Extensions;
using DoAn.Persistence.DependencyInjection.Extensions;
using DoAn.Persistence.DependencyInjection.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthenticationAPI(builder.Configuration);
builder.Services.AddMediatRApplication();

builder.Services.AddServicesInfrastructure();
builder.Services.AddRedisServiceInfrastructure(builder.Configuration);

// Persistence Layer
//dotnet ef migrations add "Init" --project DoAn.Persistence --context ApplicationDbContext --startup-project DoAn.Api --output-dir Migrations 
//dotnet ef database update --project DistributedSystem.Persistence  --startup-project DistributedSystem.Api --context ApplicationDbContext
builder.Services.AddSqlServicePersistence();
builder.Services.AddRepositoryPersistence();
builder.Services.ConfigureSqlServerRetryOptionsPersistence(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();