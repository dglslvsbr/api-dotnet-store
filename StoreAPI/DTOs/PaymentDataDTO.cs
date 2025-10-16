namespace StoreAPI.DTOs;

public class PaymentDataDTO
{
    public string? CPF { get; set; }
    public string? Number { get; set; }
    public DateTimeOffset Expiration { get; set; }
    public string? CVV { get; set; }
    public int Installments { get; set; }
}