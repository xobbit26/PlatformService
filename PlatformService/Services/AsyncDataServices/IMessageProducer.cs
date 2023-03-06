namespace PlatformService.Services.AsyncDataServices;

public interface IMessageProducer
{
    void SendMessage<T>(T message);
}