using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Validations;

public class CPFValidation
{
    public static ValidationResult Validate(object? obj)
    {
        if (obj is string s)
        {
            if (s.Length < 11)
                return new ValidationResult("Cannot have less than 11 characters");

            if (s.Length > 11)
                return new ValidationResult("Cannot have must than 11 characters");

            if (s.Any(char.IsWhiteSpace))
                return new ValidationResult("Cannot have empty spaces");

            if (s.Any(char.IsLetter))
                return new ValidationResult("Cannot have letters");

            return ValidationResult.Success!;
        }
        return new ValidationResult("The type is not valid");
    }
}