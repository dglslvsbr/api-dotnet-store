using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Validations
{
    public class InstallmentsValidation
    {
        public static ValidationResult Validate(object? obj)
        {
            if (obj is int n)
            {
                if (n < 0)
                    return new ValidationResult("The installments must be a non-negative integer.");

                if (n == 0)
                    return new ValidationResult("The installments must be greater than zero.");

                if (n > 12)
                    return new ValidationResult("The installments must be less than or equal to 12.");
            }
            return new ValidationResult("The installments must be an integer.");
        }
    }
}