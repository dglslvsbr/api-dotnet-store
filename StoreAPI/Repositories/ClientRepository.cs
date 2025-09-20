using Microsoft.EntityFrameworkCore;
using StoreAPI.AppContext;
using StoreAPI.Entities.Models;

namespace StoreAPI.Repositories
{
    public class ClientRepository : Repository<Client>
    {
        public ClientRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Client> GetByEmailAsync(string email)
        {
            return (await _context.Client
                .Include(x => x.Address)
                .Include(x => x.CreditCard)
                .Include(x => x.Order)!
                .ThenInclude(x => x.OrderItem)
                .Include(x => x.ClientRole)!
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == email))!;
        }
    }
}