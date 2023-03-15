using Microsoft.EntityFrameworkCore;
using PlatformService.Configuration;
using PlatformService.Data;
using PlatformService.Data.Repository;
using PlatformService.Services.AsyncDataServices;
using PlatformService.Services.SyncDataServices;
using PlatformService.Services.SyncDataServices.Grpc;
using PlatformService.Services.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);


//TODO: add dev configuration!!!!
// Add database configuration
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("default")).UseSnakeCaseNamingConvention());

// Add application configuration options
builder.Services.ConfigureOptions<RabbitMqConfigSetup>();
builder.Services.ConfigureOptions<CommandServiceConfigSetup>();

// Add services to the container
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

// Add Clients
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();

builder.Services.AddGrpc();
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


//TODO: Create db updater project
DbPreparation.SeedData(app, app.Environment.IsProduction());
Console.WriteLine($"CommandService Endpoint: {builder.Configuration["CommandServiceUrl"]}");


app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();


app.MapGet("/protos/platforms.proto",
    async context => { await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto")); });

app.Run();