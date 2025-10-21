using StoreAPI.Enums;
using StoreAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs;

public class UpdateOrderDTO
{
    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.IntIsValid))]
    public int Id { get; set; }
    [CustomValidation(typeof(DateValidation), nameof(DateValidation.DateTimeOffSet))]
    public DateTimeOffset CreatAt { get; set; }

    [CustomValidation(typeof(OrderStateValidation), nameof(OrderStateValidation.Validate))]
    public OrderState CurrentState { get; set; }

    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.InstallmentIsValid))]
    public int Installments { get; set; }

    [CustomValidation(typeof(NumberValidation), nameof(NumberValidation.IntIsValid))]
    public int ClientId { get; set; }
}