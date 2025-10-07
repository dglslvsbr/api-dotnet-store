using StoreAPI.Entities.Authentication;

namespace StoreAPI.Interfaces;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role> GetByNameAsync(string roleName);
}