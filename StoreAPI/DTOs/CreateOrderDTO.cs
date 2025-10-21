using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs;

public class CreateOrderDTO
{
    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.IntIsValid))]
    public int ClientId { get; set; }

    [Required(ErrorMessage = "Product List is required")]
    public List<BuyProductDTO> ProductList { get; set; } = null!;

    [Required(ErrorMessage = "Payment Data is required")]
    public PaymentDataDTO PaymentData { get; set; } = null!;
}