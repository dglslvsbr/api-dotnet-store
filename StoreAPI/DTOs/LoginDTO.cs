using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs;

public class LoginDTO
{
    [EmailAddress]
    public string? Email { get; set; }

    [CustomValidation(typeof(PasswordValidation), nameof(PasswordValidation.Validate))]
    public string? Password { get; set; }
}