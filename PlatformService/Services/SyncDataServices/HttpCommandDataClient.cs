using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PlatformService.Configuration;
using PlatformService.DTOs;

namespace PlatformService.Services.SyncDataServices;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly ILogger<HttpCommandDataClient> _logger;
    private readonly HttpClient _httpClient;
    private readonly CommandServiceConfig _commandServiceConfig;

    public HttpCommandDataClient(
        ILogger<HttpCommandDataClient> logger,
        HttpClient httpClient,
        IOptions<CommandServiceConfig> commandServiceOptions
    )
    {
        _logger = logger;
        _httpClient = httpClient;
        _commandServiceConfig = commandServiceOptions.Value;
    }

    public async Task SendPlatformToCommand(PlatformReadDto platform)
    {
        if (string.IsNullOrEmpty(_commandServiceConfig.PlatformUrl))
            throw new Exception("PlatformUrl is not configured");

        var httpContent = new StringContent(
            JsonSerializer.Serialize(platform),
            Encoding.UTF8,
            MediaTypeNames.Application.Json
        );

        var response = await _httpClient.PostAsync(_commandServiceConfig.PlatformUrl, httpContent);

        _logger.LogInformation(response.IsSuccessStatusCode
            ? "--> Sync POST to CommandService was OK"
            : "--> Sync POST to CommandService was NOT OK");
    }
}