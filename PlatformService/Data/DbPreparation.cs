using Microsoft.EntityFrameworkCore;
using PlatformService.Data.Entities;

namespace PlatformService.Data;

public static class DbPreparation
{
    public static void SeedData(IApplicationBuilder appBuilder, bool isProd)
    {
        using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
        {
            var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
            if (isProd)
            {
                SeedProdData(dbContext);
            }
            else
            {
                SeedTestData(dbContext);
            }
        }
    }

    private static void SeedProdData(AppDbContext dbContext)
    {
        Console.WriteLine("--> Attempting to apply migrations...");
        try
        {
            dbContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not run migrations: {ex.Message}");
        }
    }

    private static void SeedTestData(AppDbContext dbContext)
    {
        if (!dbContext.Platforms.Any())
        {
            Console.WriteLine($"--> Test data seeding");

            dbContext.Platforms.AddRangeAsync(new List<Platform>
            {
                new() {Id = 1, Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
                new() {Id = 2, Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free"},
                new() {Id = 3, Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free"}
            });
            dbContext.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}