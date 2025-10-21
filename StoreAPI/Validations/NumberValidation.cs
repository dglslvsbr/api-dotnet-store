using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Validations;

public class NumberValidation
{
    public static ValidationResult IntIsValid(object? obj)
    {
        if (obj is int n)
        {
            if (n <= 0)
                return new ValidationResult("The number must be greater than zero.");
            return ValidationResult.Success!;
        }
        return new ValidationResult("The number must be an integer.");
    }
    
    public static ValidationResult DecimalIsValid(object? obj)
    {
        if (obj is decimal d)
        {
            if (d <= 0m)
                return new ValidationResult("The value must be greater than zero.");
            return ValidationResult.Success!;
        }
        return new ValidationResult("The type needs to be decimal.");
    }
    
    public static ValidationResult InstallmentIsValid(object? obj)
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