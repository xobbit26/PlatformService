namespace CommandService.Services.EventProcessing;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}