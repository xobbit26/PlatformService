namespace PlatformService.Services.AsyncDataServices;

public interface IEventBus
{
    void SendMessage<T>(T message);
}