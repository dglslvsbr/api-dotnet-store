using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Validations;

public class NumberValidation
{
    public static ValidationResult IntIsValid(object? obj)
    {
        if (obj == null)
            return new ValidationResult("Int is required.");

        try
        {
            int n = Convert.ToInt32(obj);

            if (n < 0)
                return new ValidationResult("The value cannot be negative");
            else if (n == 0)
                return new ValidationResult("The value must be greater than zero");

            return ValidationResult.Success!;
        }
        catch
        {
            return new ValidationResult("The number is not valid");
        }
    }
    
    public static ValidationResult DecimalIsValid(object? obj)
    {
        if (obj == null)
            return new ValidationResult("Decimal is required.");

        try
        {
            decimal n = Convert.ToDecimal(obj);

            if (n < 0m)
                return new ValidationResult("The value cannot be negative");
            else if (n == 0m)
                return new ValidationResult("The value must be greater than zero");

            return ValidationResult.Success!;
        }
        catch
        {
            return new ValidationResult("The number is not valid");
        }
    }

    public static ValidationResult InstallmentIsValid(object? obj)
    {
        if (obj == null)
            return new ValidationResult("Installments is required.");

        try
        {
            int n = Convert.ToInt32(obj);

            if (n < 0)
                return new ValidationResult("The installments must be a non-negative integer.");
            if (n == 0)
                return new ValidationResult("The installments must be greater than zero.");
            if (n > 12)
                return new ValidationResult("The installments must be less than or equal to 12.");

            return ValidationResult.Success!;
        }
        catch
        {
            return new ValidationResult("The installments must be an integer.");
        }
    }
}