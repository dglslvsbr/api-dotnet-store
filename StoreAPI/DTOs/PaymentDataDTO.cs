using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs;

public class PaymentDataDTO
{
    [CustomValidation(typeof(StringValidation), nameof(StringValidation.CpfAndPhone))]
    public string? CPF { get; set; }

    [CustomValidation(typeof(StringValidation), nameof(StringValidation.NumberCard))]
    public string? Number { get; set; }

    [CustomValidation(typeof(DateValidation), nameof(DateValidation.DateTimeOffSet))]
    public DateTimeOffset Expiration { get; set; }

    [CustomValidation(typeof(StringValidation), nameof(StringValidation.CVV))]
    public string? CVV { get; set; }

    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.InstallmentIsValid))]
    public int Installments { get; set; }
}