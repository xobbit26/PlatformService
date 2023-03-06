using Microsoft.Extensions.Options;

namespace PlatformService.Configuration;

public class CommandServiceConfigSetup : IConfigureOptions<CommandServiceConfig>
{
    private const string ConfigurationSectionName = "CommandServiceOptions";
    private readonly IConfiguration _appConfig;

    public CommandServiceConfigSetup(IConfiguration appConfig)
    {
        _appConfig = appConfig;
    }

    public void Configure(CommandServiceConfig config)
    {
        _appConfig
            .GetSection(ConfigurationSectionName)
            .Bind(config);
    }
}

public class CommandServiceConfig
{
    public string BaseUrl { get; set; }
    public string PlatformUrl { get; set; }
}