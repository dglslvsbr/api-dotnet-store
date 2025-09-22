using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Validations
{
    public class StringValidation
    {
        public static ValidationResult Validate(object? obj)
        {
            if (obj is string text)
            {
                if (text.Length < 5)
                    return new ValidationResult("Cannot have less than 5 characters");

                if (text.Any(char.IsDigit))
                    return new ValidationResult("Cannot have numbers");

                if (text.Any(char.IsWhiteSpace))
                    return new ValidationResult("Cannot have empty spaces");

                if (text.Length > 30)
                    return new ValidationResult("Cannot have must than 5 characters");

                return ValidationResult.Success!;
            }
            return new ValidationResult("The type is not valid");
        }

    }
}