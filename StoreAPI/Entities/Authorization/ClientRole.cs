using StoreAPI.Entities.Models;
using System.Text.Json.Serialization;

namespace StoreAPI.Entities.Authentication;

public class ClientRole
{
    public int ClientId { get; set; }
    [JsonIgnore]
    public Client? Client { get; set; }
    public int RoleId { get; set; }
    [JsonIgnore]
    public Role? Role { get; set; }
}