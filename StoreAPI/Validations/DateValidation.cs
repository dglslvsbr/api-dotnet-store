using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Validations;

public class DateValidation
{
    public static ValidationResult DateTimeOffSet(object? obj)
    {
        if (obj is DateTimeOffset date)
        {
            if (date < DateTimeOffset.UtcNow)
                return new ValidationResult("The date cannot be in the past.");

            return ValidationResult.Success!;
        }

        return new ValidationResult("Invalid data type. Expected DateTimeOffset.");
    }
}