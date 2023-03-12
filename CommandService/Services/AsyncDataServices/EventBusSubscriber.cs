using System.Text;
using CommandService.Configuration;
using CommandService.Services.EventProcessing;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.Services.AsyncDataServices;

public class EventBusSubscriber : BackgroundService
{
    // private const string QueueName = "platforms";

    private readonly ILogger<EventBusSubscriber> _logger;
    private readonly RabbitMqConfig _rabbitMqConfig;
    private readonly IEventProcessor _eventProcessor;

    private ConnectionFactory _factory;
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;


    public EventBusSubscriber(
        ILogger<EventBusSubscriber> logger,
        IOptions<RabbitMqConfig> rabbitMqOptions,
        IEventProcessor eventProcessor
    )
    {
        _logger = logger;
        _eventProcessor = eventProcessor;
        _rabbitMqConfig = rabbitMqOptions.Value;

        InitializeRabbitMq();
    }

    private void InitializeRabbitMq()
    {
        _logger.LogInformation($"RabbitMqOptions host: {_rabbitMqConfig.Host}, port: {_rabbitMqConfig.Port}");

        //Initialize RabbitMQ
        _factory = new ConnectionFactory {HostName = _rabbitMqConfig.Host, Port = _rabbitMqConfig.Port};
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;

        _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");

        _connection.ConnectionShutdown += (sender, args)
            => _logger.LogInformation("Connection Shutdown");


        _logger.LogInformation("Listening on the Message Bus...");
    }


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (sender, args) =>
        {
            _logger.LogInformation("Event received!");

            var body = args.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());

            _eventProcessor.ProcessEvent(message);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }

        base.Dispose();
    }
}