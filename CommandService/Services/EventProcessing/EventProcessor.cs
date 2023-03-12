using System.Text.Json;
using AutoMapper;
using CommandService.Data.Entities;
using CommandService.Data.Repos;
using CommandService.Events;

namespace CommandService.Services.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;
    private readonly ILogger<EventProcessor> _logger;

    public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper, ILogger<EventProcessor> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _mapper = mapper;
        _logger = logger;
    }

    public Task ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                AddPlatform(message);
                break;
            default:
                break;
        }

        throw new NotImplementedException();
    }

    private EventType DetermineEvent(string message)
    {
        _logger.LogInformation("Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEvent>(message);

        switch (eventType.Event)
        {
            case "Platform_Published":
                _logger.LogInformation("Platform Published Event Detected");
                return EventType.PlatformPublished;
            default:
                _logger.LogInformation("Could not determine the event type");
                return EventType.Undefined;
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

        var platformPublishedEvent = JsonSerializer.Deserialize<PlatformPublishedEvent>(platformPublishedMessage);

        try
        {
            var platform = _mapper.Map<Platform>(platformPublishedEvent);

            if (!repo.ExternalPlatformExists(platform.ExternalId))
            {
                repo.CreatePlatform(platform);
                repo.SaveChanges();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not add Platform to DB {e.Message}");
        }
    }
}

internal enum EventType
{
    PlatformPublished,
    Undefined
}