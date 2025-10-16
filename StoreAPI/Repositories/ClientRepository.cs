using Microsoft.EntityFrameworkCore;
using StoreAPI.Context;
using StoreAPI.DTOs;
using StoreAPI.Entities.Models;
using StoreAPI.Enums;
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
           .ThenInclude(x => x.OrderItem!)
           .ThenInclude(x => x.Product)
           .Include(x => x.ClientRole)!
           .ThenInclude(x => x.Role).AsSplitQuery()
           .FirstOrDefaultAsync(x => x.Email == email))!;
    }

    public async Task<Client?> GetByCPFAsync(string cpf)
    {
        return (await _context.Client
           .Include(x => x.Address)
           .Include(x => x.CreditCard)
           .Include(x => x.Order)!
           .ThenInclude(x => x.OrderItem)
           .Include(x => x.ClientRole)!
           .ThenInclude(x => x.Role).AsSplitQuery()
           .FirstOrDefaultAsync(x => x.CPF == cpf))!;
    }

    public async Task<List<DuplicateField>> CheckDuplicates(CreateClientDTO client)
    {
        var duplicates = await _context.Client.Where(x => x.Email == client.Email || x.CPF == client.CPF || x.Phone == client.Phone)
            .Select(x => new
            {
                EmailExist = x.Email == client.Email,
                CPFExist = x.CPF == client.CPF,
                PhoneExist = x.Phone == client.Phone
            }).ToListAsync();

        var result = new List<DuplicateField>();

        if (duplicates.Any(x => x.EmailExist))
            result.Add(DuplicateField.Email_Exist);

        if (duplicates.Any(x => x.CPFExist))
            result.Add(DuplicateField.CPF_Exist);

        if (duplicates.Any(x => x.PhoneExist))
            result.Add(DuplicateField.Phone_Exist);

        return result;
    }
}