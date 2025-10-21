using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs;

public class UpdateClientDTO
{
    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.IntIsValid))]
    public int Id { get; set; }
    [CustomValidation(typeof(StringValidation), nameof(StringValidation.TextNoNumber))]
    public string? FirstName { get; set; }

    [CustomValidation(typeof(StringValidation), nameof(StringValidation.TextNoNumber))]
    public string? LastName { get; set; }

    [EmailAddress(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [CustomValidation(typeof(StringValidation), nameof(StringValidation.TextNoWhiteSpace))]
    public string? Password { get; set; }

    [CustomValidation(typeof(StringValidation), nameof(StringValidation.CpfAndPhone))]
    public string? CPF { get; set; }

    [CustomValidation(typeof(StringValidation), nameof(StringValidation.CpfAndPhone))]
    public string? Phone { get; set; }
}