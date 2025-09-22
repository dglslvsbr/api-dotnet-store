using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Validations
{
    public class DecimalValidation
    {
        public static ValidationResult Validate(object? obj)
        {
            if (obj is decimal d)
            {
                if (d <= 0m)
                    return new ValidationResult("The value must be greater than zero.");
                return ValidationResult.Success!;
            }
            return new ValidationResult("The type needs to be decimal.");
        }
    }
}
