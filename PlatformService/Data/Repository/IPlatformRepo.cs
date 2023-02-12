using PlatformService.Data.Entities;

namespace PlatformService.Data.Repository;

public interface IPlatformRepo
{
    Task<IEnumerable<Platform>> GetAll();
    Task<Platform> GetById(int id);
    Task<bool> Create(Platform platform);
}