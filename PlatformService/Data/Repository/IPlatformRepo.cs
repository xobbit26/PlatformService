using PlatformService.Data.Entities;

namespace PlatformService.Data.Repository;

public interface IPlatformRepo
{
    Task<bool> SaveChanges();
    Task<IEnumerable<Platform>> GetAll();
    Task<Platform> GetById(int id);
    void Create(Platform platform);
}