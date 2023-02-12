using PlatformService.DTOs;

namespace PlatformService.SyncDataServices;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformReadDto plat);
}