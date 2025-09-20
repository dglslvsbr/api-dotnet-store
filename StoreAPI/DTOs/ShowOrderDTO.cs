using StoreAPI.Entities.Models;
using StoreAPI.Enums;

namespace StoreAPI.DTOs
{
    public class ShowOrderDTO
    {
        public int Id { get; set; }
        public DateTimeOffset CreatAt { get; set; }
        public OrderState CurrentState { get; set; }
        public int Installments { get; set; }
        public int ClientId { get; set; }
        public IEnumerable<OrderItem>? OrderItem { get; set; }
    }
}