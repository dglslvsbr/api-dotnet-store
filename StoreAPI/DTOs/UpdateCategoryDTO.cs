using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs;

public class UpdateCategoryDTO
{
    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.IntIsValid))]
    public int Id { get; set; }

    [CustomValidation(typeof(StringValidation), nameof(StringValidation.TextNoNumber))]
    public string? Name { get; set; }
}