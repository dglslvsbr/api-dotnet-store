using Microsoft.EntityFrameworkCore;
using StoreAPI.Context;
using StoreAPI.Entities.Models;
using StoreAPI.Interfaces;

namespace StoreAPI.Repositories;

public class ClientRepository(AppDbContext context) : Repository<Client>(context), IClientRepository
{
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