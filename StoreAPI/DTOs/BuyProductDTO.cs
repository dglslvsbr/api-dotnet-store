using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs;

public class BuyProductDTO
{
    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.IntIsValid))]
    public int ProductId { get; set; }

    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.IntIsValid))]
    public int Quantity { get; set; }
}