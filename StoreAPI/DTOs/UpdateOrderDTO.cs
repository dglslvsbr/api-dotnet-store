using StoreAPI.Enums;
using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs;

public class UpdateOrderDTO
{
    [Required]
    public int Id { get; set; }
    [CustomValidation(typeof(CurrentDataValidation), nameof(CurrentDataValidation.Validate))]
    public DateTimeOffset CreatAt { get; set; }

    [CustomValidation(typeof(OrderStateValidation), nameof(OrderStateValidation.Validate))]
    public OrderState CurrentState { get; set; }

    [CustomValidation(typeof(InstallmentsValidation), nameof(InstallmentsValidation.Validate))]
    public int Installments { get; set; }

    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.Validate))]
    public int ClientId { get; set; }
}