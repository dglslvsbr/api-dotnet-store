using StoreAPI.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Validations;

public class DomainModelValidate
{
    public static ValidationResult AddressValidate(object? obj)
    {
        if (obj is Address address)
        {
            var properties = new List<string>()
            {
                address.Street!,
                address.Number!,
                address.District!,
                address.City!,
                address.State!
            };

            foreach (var property in properties)
            {
                if (string.IsNullOrEmpty(property))
                    return new ValidationResult("Property cannot be empty");

                if (property.Length < 5)
                    return new ValidationResult("Property must have at least 5 characters");

                if (property.Length > 50)
                    return new ValidationResult("Property cannot have more than 50 characters");

                if (property.Any(char.IsWhiteSpace))
                    return new ValidationResult("Property cannot have empty spaces");

                return ValidationResult.Success!;
            }
        }
        return new ValidationResult("Address properties are not valid");
    }
}
