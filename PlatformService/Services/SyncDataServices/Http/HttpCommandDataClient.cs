using System.Net.Mime;
using System.Text;
using System.Text.Json;
using PlatformService.DTOs;

namespace PlatformService.Services.SyncDataServices.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task SendPlatformToCommand(PlatformReadDto platform)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(platform),
            Encoding.UTF8,
            MediaTypeNames.Application.Json
        );

        var response =
            await _httpClient.PostAsync($"{_configuration["CommandServiceUrl"]}/api/c/platform", httpContent);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "--> Sync POST to CommandService was OK"
            : "--> Sync POST to CommandService was NOT OK");
    }
}