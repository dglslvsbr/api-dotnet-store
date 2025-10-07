namespace StoreAPI.Entities.Models;

public class CreditCard
{
    public int Id { get; set; }
    public string? Number { get; set; }
    public DateTimeOffset Expiration { get; set; }
    public string? CVV { get; set; }     
    public decimal? UsedLimit { get; set; }
    public decimal MaxLimit { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }

    public decimal RemainingLimit => (decimal)(MaxLimit - UsedLimit)!;
}