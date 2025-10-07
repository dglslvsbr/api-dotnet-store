using Microsoft.EntityFrameworkCore;
using StoreAPI.Context;
using StoreAPI.Entities.Authentication;
using StoreAPI.Interfaces;

namespace StoreAPI.Repositories;

public class RoleRepository(AppDbContext context) : Repository<Role>(context), IRoleRepository
{
    public async Task<Role> GetByNameAsync(string roleName)
    {
        return (await _context.Role.FirstOrDefaultAsync(x => x.Name == roleName))!;
    }
}