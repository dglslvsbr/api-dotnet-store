using StoreAPI.Entities.Authentication;

namespace StoreAPI.Entities.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? CPF { get; set; }
        public string? Phone { get; set; }
        public Address? Address { get; set; }
        public CreditCard? CreditCard { get; set; }
        public ICollection<Order>? Order { get; set; }
        public ICollection<ClientRole>? ClientRole { get; set; }
    }
}