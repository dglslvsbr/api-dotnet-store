using StoreAPI.Entities.Models;
using StoreAPI.Enums;

namespace StoreAPI.DTOs
{
    public class CreateOrderDTO
    {
        public DateTimeOffset CreatAt { get; set; }
        public OrderState CurrentState { get; set; }
        public int Installments { get; set; }
        public int ClientId { get; set; }
    }
}