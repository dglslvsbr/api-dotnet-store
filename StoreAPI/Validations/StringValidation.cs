using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Validations;

public class StringValidation
{
    public static ValidationResult TextNoNumber(object? obj)
    {
        if (obj is string text)
        {
            if (text.Length < 5)
                return new ValidationResult("Cannot have less than 5 characters");

            if (text.Any(char.IsDigit))
                return new ValidationResult("Cannot have numbers");

            if (text.Any(char.IsWhiteSpace))
                return new ValidationResult("Cannot have empty spaces");

            if (text.Length > 100)
                return new ValidationResult("Cannot have must than 100 characters");

            return ValidationResult.Success!;
        }
        return new ValidationResult("The type is not valid");
    }
    
    public static ValidationResult Text(object? obj)
    {
        if (obj is string text)
        {
            if (text.Length < 5)
                return new ValidationResult("Cannot have less than 5 characters");

            if (text.Length > 100)
                return new ValidationResult("Cannot have must than 100 characters");

            return ValidationResult.Success!;
        }
        return new ValidationResult("The type is not valid");
    }
    
    public static ValidationResult CpfAndPhone(object? obj)
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
    
    public static ValidationResult TextNoWhiteSpace(object? obj)
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

    public static ValidationResult NumberCard(object? obj)
    {
        if (obj is string s)
        {
            if (s.Length < 16)
                return new ValidationResult("Cannot have less than 16 characters");

            if (s.Length > 16)
                return new ValidationResult("Cannot have must than 16 characters");

            if (s.Any(char.IsWhiteSpace))
                return new ValidationResult("Cannot have empty spaces");

            if (s.Any(char.IsLetter))
                return new ValidationResult("Cannot have letters");

            return ValidationResult.Success!;
        }
        return new ValidationResult("The type is not valid");
    }

    public static ValidationResult CVV(object? obj)
    {
        if (obj is string s)
        {
            if (s.Length < 3)
                return new ValidationResult("Cannot have less than 3 characters");

            if (s.Length > 3)
                return new ValidationResult("Cannot have must than 3 characters");

            if (s.Any(char.IsWhiteSpace))
                return new ValidationResult("Cannot have empty spaces");

            if (s.Any(char.IsLetter))
                return new ValidationResult("Cannot have letters");

            return ValidationResult.Success!;
        }
        return new ValidationResult("The type is not valid");
    }
}