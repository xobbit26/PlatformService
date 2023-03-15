using PlatformService.DTOs;

namespace PlatformService.Services.SyncDataServices.Http;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformReadDto platform);
}