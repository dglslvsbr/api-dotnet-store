using System.Text.Json.Serialization;

namespace StoreAPI.Entities.Models;

public class Address
{
    [JsonIgnore]
    public int Id { get; set; }
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? District { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    [JsonIgnore]
    public int ClientId { get; set; }
    [JsonIgnore]
    public Client? Client { get; set; }
}