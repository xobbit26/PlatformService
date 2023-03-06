using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Data.Repository;
using PlatformService.Services.SyncDataServices;
using PlatformService.Services.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Environment.IsProduction()
    ? builder.Configuration.GetConnectionString("prod")
    : builder.Configuration.GetConnectionString("dev");

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString)
    .UseSnakeCaseNamingConvention());

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

// Add Http Clients
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();


DbPreparation.SeedData(app, app.Environment.IsProduction());
Console.WriteLine($"CommandService Endpoint: {builder.Configuration["CommandServiceUrl"]}");


app.MapControllers();
app.Run();