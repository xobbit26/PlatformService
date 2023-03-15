using CommandService.Data.Entities;
using CommandService.Data.Repos;
using CommandService.Services.SyncDataServices;

namespace CommandService.Data;

public static class DbPreparation
{
    public static void SeedData(IApplicationBuilder appBuilder)
    {
        using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
        {
            var commandRepo = serviceScope.ServiceProvider.GetService<ICommandRepo>();
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
            var platforms = grpcClient.GetAllPlatforms();

            SeedTestData(commandRepo, platforms);
        }
    }

    private static void SeedTestData(ICommandRepo repo, IEnumerable<Platform> platforms)
    {
        Console.WriteLine("Seeding new Platforms...");


        foreach (var platform in platforms)
        {
            if (!repo.ExternalPlatformExists(platform.ExternalId))
            {
                repo.CreatePlatform(platform);
            }

            repo.SaveChanges();
        }
    }
}