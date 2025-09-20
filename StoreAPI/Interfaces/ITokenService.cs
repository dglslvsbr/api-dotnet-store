using StoreAPI.Entities.Models;

namespace StoreAPI.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(Client client, IConfiguration configuration);
    }
}