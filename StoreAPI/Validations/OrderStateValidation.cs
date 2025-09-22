using StoreAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Validations
{
    public class OrderStateValidation
    {
        public static ValidationResult Validate(object? obj)
        {
            if (obj is OrderState)
            {
                if (Enum.IsDefined(typeof(OrderState), obj))
                    return ValidationResult.Success!;

                return new ValidationResult("The order state is not valid.");
            }
            return new ValidationResult("Invalid order state.");
        }
    }
}