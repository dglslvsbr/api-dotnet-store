using StoreAPI.DTOs;
using StoreAPI.Entities.Authentication;

namespace StoreAPI.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<CreateRoleDTO>> GetAllAsync();
    Task<CreateRoleDTO> GetAsync(int id);
    Task<CreateRoleDTO> GetByNameAsync(string name);
    Task CreateAsync(Role entity);
    Task UpdateAsync(Role entity);
    Task DeleteAsync(Role entity);
}