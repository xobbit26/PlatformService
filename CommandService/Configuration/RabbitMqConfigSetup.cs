using Microsoft.Extensions.Options;

namespace CommandService.Configuration;

public class RabbitMqConfigSetup : IConfigureOptions<RabbitMqConfig>
{
    private const string ConfigurationSectionName = "RabbitMqOptions";
    private readonly IConfiguration _appConfig;

    public RabbitMqConfigSetup(IConfiguration appConfig)
    {
        _appConfig = appConfig;
    }

    public void Configure(RabbitMqConfig config)
    {
        _appConfig
            .GetSection(ConfigurationSectionName)
            .Bind(config);
    }
}

public class RabbitMqConfig
{
    public string Host { get; set; }
    public int Port { get; set; }
}