using StoreAPI.Entities.Models;

namespace StoreAPI.Interfaces;

public interface IClientRepository : IRepository<Client>
{
    Task<Client> GetByEmailAsync(string email);
}