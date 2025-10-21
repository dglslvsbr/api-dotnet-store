using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs;

public class LoginDTO
{
    [EmailAddress(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [CustomValidation(typeof(StringValidation), nameof(StringValidation.TextNoWhiteSpace))]
    public string? Password { get; set; }
}