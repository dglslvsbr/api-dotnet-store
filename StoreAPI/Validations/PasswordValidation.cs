using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Validations
{
    public class PasswordValidation
    {
        public static ValidationResult Validate(object? obj)
        {
            if (obj is string s)
            {
                if (s.Length < 5)
                    return new ValidationResult("Cannot have less than 5 characters");

                if (s.Length > 30)
                    return new ValidationResult("Cannot have must than 30 characters");

                if (s.Any(char.IsWhiteSpace))
                    return new ValidationResult("Cannot have empty spaces");

                return ValidationResult.Success!;
            }
            return new ValidationResult("The type is not valid");
        }
    }
}