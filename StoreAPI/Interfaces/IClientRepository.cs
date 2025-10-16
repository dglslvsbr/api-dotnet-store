using StoreAPI.DTOs;
using StoreAPI.Entities.Models;
using StoreAPI.Enums;

namespace StoreAPI.Interfaces;

public interface IClientRepository : IRepository<Client>
{
    Task<Client> GetByEmailAsync(string email);
    Task<Client?> GetByCPFAsync(string cpf);
    Task<List<DuplicateField>> CheckDuplicates(CreateClientDTO client);
}