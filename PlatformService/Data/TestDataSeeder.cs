using Microsoft.EntityFrameworkCore;
using PlatformService.Data.Entities;

namespace PlatformService.Data;

public static class TestDataSeeder
{
    public static void SeedTestData(IApplicationBuilder appBuilder)
    {
        using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
        {
            var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
            SeedTestData(dbContext);
        }
    }

    private static void SeedTestData(AppDbContext dbContext)
    {
        SeedPlatforms(dbContext.Platforms);
        dbContext.SaveChanges();
    }

    private static void SeedPlatforms(DbSet<Platform> platforms)
    {
        if (!platforms.Any())
        {
            platforms.AddRangeAsync(new List<Platform>
            {
                new() {Id = 1, Name = "test_platform_1", Publisher = "test_publisher_1", Cost = "test_cost_1"},
                new() {Id = 2, Name = "test_platform_2", Publisher = "test_publisher_2", Cost = "test_cost_2"},
                new() {Id = 3, Name = "test_platform_3", Publisher = "test_publisher_3", Cost = "test_cost_3"}
            });
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}