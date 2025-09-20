using Microsoft.EntityFrameworkCore;
using StoreAPI.AppContext;
using StoreAPI.Entities.Authentication;

namespace StoreAPI.Repositories
{
    public class RoleRepository : Repository<Role>
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Role> GetByNameAsync(string roleName)
        {
            return (await _context.Role.FirstOrDefaultAsync(x => x.Name == roleName))!;
        }
    }
}