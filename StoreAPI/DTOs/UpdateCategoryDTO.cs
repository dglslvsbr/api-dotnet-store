using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs;

public class UpdateCategoryDTO
{
    public int Id { get; set; }
    [CustomValidation(typeof(StringValidation), nameof(StringValidation.Validate))]
    public string? Name { get; set; }
}