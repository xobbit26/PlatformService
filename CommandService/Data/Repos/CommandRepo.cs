using CommandService.Data.Entities;

namespace CommandService.Data.Repos;

public class CommandRepo : ICommandRepo
{
    private readonly AppDbContext _dbContext;

    public CommandRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool SaveChanges()
    {
        return _dbContext.SaveChanges() > 0;
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _dbContext.Platforms.ToList();
    }

    public void CreatePlatform(Platform platform)
    {
        if (platform == null)
            throw new ArgumentNullException(nameof(platform));

        _dbContext.Platforms.Add(platform);
    }

    public bool IsPlatformExists(int platformId)
    {
        return _dbContext.Platforms.Any(p => p.Id == platformId);
    }

    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
    {
        return _dbContext.Commands
            .Where(c => c.PlatformId == platformId)
            .OrderBy(c => c.Platform.Name);
    }

    public Command GetCommand(int platformId, int commandId)
    {
        return _dbContext.Commands.FirstOrDefault(c => c.PlatformId == platformId && c.Id == commandId);
    }

    public void CreateCommand(int platformId, Command command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        command.PlatformId = platformId;
        _dbContext.Commands.Add(command);
    }
}