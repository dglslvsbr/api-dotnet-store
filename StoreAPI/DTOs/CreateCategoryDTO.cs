
using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs;

public class CreateCategoryDTO
{
    [CustomValidation(typeof(StringValidation), nameof(StringValidation.TextNoNumber))]
    public string? Name { get; set; }
}