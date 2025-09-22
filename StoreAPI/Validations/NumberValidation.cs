using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Validations
{
    public class NumberValidation
    {
        public static ValidationResult Validate(object? obj)
        {
            if (obj is int n)
            {
                if (n <= 0)
                    return new ValidationResult("The number must be greater than zero.");
                return ValidationResult.Success!;
            }
            return new ValidationResult("The number must be an integer.");
        }
    }
}