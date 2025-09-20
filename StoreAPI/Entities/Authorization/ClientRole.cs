using StoreAPI.Entities.Models;

namespace StoreAPI.Entities.Authentication
{
    public class ClientRole
    {
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}