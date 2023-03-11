using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PlatformService.Configuration;
using RabbitMQ.Client;

namespace PlatformService.Services.AsyncDataServices;

public class RabbitMqProducer : IMessageProducer
{
    // private const string QueueName = "platforms";

    private readonly ILogger<RabbitMqProducer> _logger;
    private readonly RabbitMqConfig _rabbitMqConfig;

    public RabbitMqProducer(ILogger<RabbitMqProducer> logger, IOptions<RabbitMqConfig> rabbitMqOptions)
    {
        _logger = logger;
        _rabbitMqConfig = rabbitMqOptions.Value;

        _logger.LogInformation($"RabbitMqOptions host: {_rabbitMqConfig.Host}, port: {_rabbitMqConfig.Port}");
    }

    public void SendMessage<T>(T message)
    {
        if (string.IsNullOrEmpty(_rabbitMqConfig.Host) || _rabbitMqConfig.Port == 0)
            throw new Exception("Host or Port for rabbitMq are not configured");

        var factory = new ConnectionFactory {HostName = _rabbitMqConfig.Host, Port = _rabbitMqConfig.Port};
        var connection = factory.CreateConnection();

        using var channel = connection.CreateModel();
        // channel.QueueDeclare(QueueName);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(
            exchange: ExchangeType.Fanout,
            // routingKey: QueueName,
            routingKey: string.Empty,
            body: body);

        _logger.LogInformation("Message has been published to message bus");
    }
}