using StoreAPI.Entities.Authentication;
using StoreAPI.Entities.Models;
using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StoreAPI.DTOs;

public class CreateClientDTO
{
    [CustomValidation(typeof(StringValidation), nameof(StringValidation.Validate))]
    public string? FirstName { get; set; }

    [CustomValidation(typeof(StringValidation), nameof(StringValidation.Validate))]
    public string? LastName { get; set; }

    [EmailAddress(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [CustomValidation(typeof(PasswordValidation), nameof(PasswordValidation.Validate))]
    public string? Password { get; set; }

    [CustomValidation(typeof(CPFValidation), nameof(CPFValidation.Validate))]
    public string? CPF { get; set; }

    [CustomValidation(typeof(PhoneValidation), nameof(PhoneValidation.Validate))]
    public string? Phone { get; set; }
    [Required(ErrorMessage = "Address is required")]
    public Address Address { get; set; } = null!;
    [JsonIgnore]
    public CreditCard? CreditCard { get; set; }
    [JsonIgnore]
    public ICollection<ClientRole>? ClientRole { get; set; }
}