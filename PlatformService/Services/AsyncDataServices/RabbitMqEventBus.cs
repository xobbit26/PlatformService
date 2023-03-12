using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PlatformService.Configuration;
using RabbitMQ.Client;

namespace PlatformService.Services.AsyncDataServices;

public class RabbitMqEventBus : IEventBus, IDisposable
{
    // private const string QueueName = "platforms";

    private readonly ILogger<RabbitMqEventBus> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqEventBus(ILogger<RabbitMqEventBus> logger, IOptions<RabbitMqConfig> rabbitMqOptions)
    {
        _logger = logger;
        var rabbitMqConfig = rabbitMqOptions.Value;

        _logger.LogInformation($"RabbitMqOptions host: {rabbitMqConfig.Host}, port: {rabbitMqConfig.Port}");

        if (string.IsNullOrEmpty(rabbitMqConfig.Host) || rabbitMqConfig.Port == 0)
        {
            var ex = new Exception("Host or Port for rabbitMq are not configured");
            _logger.LogError(ex, ex.Message);
            throw ex;
        }

        try
        {
            var factory = new ConnectionFactory {HostName = rabbitMqConfig.Host, Port = rabbitMqConfig.Port};
            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            _connection.ConnectionShutdown += (sender, args)
                => logger.LogInformation("RabbitMQ Connection Shutdown");

            logger.LogInformation("Connected to Message Bus");
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Could not connect to message bus : {e.Message}");
        }
    }

    public void SendMessage<T>(T message)
    {
        if (_connection.IsOpen)
        {
            _logger.LogInformation("RabbitMq Connection Open, sending message...");

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);

            _logger.LogInformation($"Message has been sent: {json}");
        }
        else
        {
            _logger.LogInformation("RabbitMq Connection Closed, not sending");
        }

        _logger.LogInformation("Message has been published to message bus");
    }

    public void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();

            _logger.LogInformation("Message Bus disposed");
        }
    }
}