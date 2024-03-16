

using DoAn.Application.DependencyInjection.Extensions;
using DoAn.Persistence.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMediatRApplication();

// Persistence Layer
//dotnet ef migrations add "Init" --project DoAn.Persistence --context ApplicationDbContext --startup-project DoAn.Api --output-dir Migrations 
//dotnet ef database update --project DistributedSystem.Persistence  --startup-project DistributedSystem.Api --context ApplicationDbContext
builder.Services.AddSqlServicePersistence();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();