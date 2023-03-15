using CommandService.Data.Entities;

namespace CommandService.Services.SyncDataServices;

public interface IPlatformDataClient
{
    IEnumerable<Platform> GetAllPlatforms();
}