using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs;

public class UpdateProductDTO
{
    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.IntIsValid))]
    public int Id { get; set; }

    [CustomValidation(typeof(StringValidation), nameof(StringValidation.TextNoNumber))]
    public string? Name { get; set; }

    [CustomValidation(typeof(StringValidation), nameof(StringValidation.Text))]
    public string? Description { get; set; }

    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.DecimalIsValid))]
    public decimal Price { get; set; }

    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.IntIsValid))]
    public int Quantity { get; set; }

    [CustomValidation(typeof(StringValidation), nameof(StringValidation.TextNoWhiteSpace))]
    public string? ImageUrl { get; set; }

    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.IntIsValid))]
    public int CategoryId { get; set; }
}