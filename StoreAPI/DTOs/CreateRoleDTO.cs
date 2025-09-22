using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs
{
    public class CreateRoleDTO
    {
        [CustomValidation(typeof(StringValidation), nameof(StringValidation.Validate))]
        public string? Name { get; set; }
    }
}