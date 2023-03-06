using PlatformService.DTOs;

namespace PlatformService.Services.SyncDataServices;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformReadDto platform);
}