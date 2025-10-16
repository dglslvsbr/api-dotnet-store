using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs;

public class CreateOrderDTO
{
    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.Validate))]
    public int ClientId { get; set; }
    public List<BuyProductDTO> ProductList { get; set; } = null!;
    public PaymentDataDTO PaymentData { get; set; } = null!;
}