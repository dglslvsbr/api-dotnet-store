using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs
{
    public class CreateProductDTO
    {
        [CustomValidation(typeof(StringValidation), nameof(StringValidation.Validate))]
        public string? Name { get; set; }

        [CustomValidation(typeof(StringValidation), nameof(StringValidation.Validate))]
        public string? Description { get; set; }

        [CustomValidation(typeof(DecimalValidation), nameof(DecimalValidation.Validate))]
        public decimal Price { get; set; }

        [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.Validate))]
        public int Quantity { get; set; }

        [CustomValidation(typeof(StringValidation), nameof(StringValidation.Validate))]
        public string? ImageUrl { get; set; }

        [CustomValidation(typeof(StringValidation), nameof(StringValidation.Validate))]
        public int CategoryId { get; set; }
    }
}