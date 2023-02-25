using CommandService.Data.Entities;

namespace CommandService.Data.Repos;

public interface ICommandRepo
{
    bool SaveChanges();

    //platforms
    IEnumerable<Platform> GetAllPlatforms();
    void CreatePlatform(Platform platform);
    bool IsPlatformExists(int platformId);

    //commands
    IEnumerable<Command> GetCommandsForPlatform(int platformId);
    Command GetCommand(int platformId, int commandId);
    void CreateCommand(int platformId, Command command);
}