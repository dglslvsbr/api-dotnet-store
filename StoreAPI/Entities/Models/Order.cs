using StoreAPI.Enums;

namespace StoreAPI.Entities.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTimeOffset CreatAt { get; set; }
        public OrderState CurrentState { get; set; }
        public int Installments { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public ICollection<OrderItem>? OrderItem { get; set; }

        public decimal OrderTotal => OrderItem?.Sum(x => x.Quantity * x.UnitPrice) ?? 0;
    }
}