using StoreAPI.DTOs;

namespace StoreAPI.Interfaces;

public interface IClientService
{
    Task<IEnumerable<ShowClientDTO>> GetAllAsync();
    Task<ShowClientDTO> GetAsync(int id);
    Task<ShowClientDTO> GetByEmailAsync(string email);
    Task CreateAsync(CreateClientDTO entity);
    Task UpdateAsync(UpdateClientDTO entity);
    Task DeleteAsync(ShowClientDTO entity);
}