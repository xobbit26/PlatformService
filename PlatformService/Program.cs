using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Data.Repository;
using PlatformService.SyncDataServices;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
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

//populate test database by test data 
TestDataSeeder.SeedTestData(app);
Console.WriteLine($"CommandService Endpoint: {builder.Configuration["CommandService"]}");

// app.UseHttpsRedirection();
// app.UseAuthorization();

app.MapControllers();

app.Run();