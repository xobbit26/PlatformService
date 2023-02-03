using Microsoft.EntityFrameworkCore;
using PlatformService.Data.Entities;

namespace PlatformService.Data.Repository;

public class PlatformRepo : IPlatformRepo
{
    private readonly AppDbContext _dbContext;

    public PlatformRepo(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<IEnumerable<Platform>> GetAll()
        => await _dbContext.Platforms.ToListAsync();

    public async Task<Platform> GetById(int id)
        => await _dbContext.Platforms.FirstOrDefaultAsync(p => p.Id == id);

    public void Create(Platform platform)
        => _dbContext.Platforms.Add(platform);

    public async Task<bool> SaveChanges()
    {
        var savedEntitiesCount = await _dbContext.SaveChangesAsync();
        return savedEntitiesCount > 0;
    }
}