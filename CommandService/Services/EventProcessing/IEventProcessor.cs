namespace CommandService.Services.EventProcessing;

public interface IEventProcessor
{
    Task ProcessEvent(string message);
}