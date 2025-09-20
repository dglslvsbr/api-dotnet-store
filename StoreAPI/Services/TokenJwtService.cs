using Microsoft.IdentityModel.Tokens;
using StoreAPI.Entities.Models;
using StoreAPI.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreAPI.Services
{
    public class TokenJwtService : ITokenService
    {
        public string GenerateToken(Client client, IConfiguration configuration)
        {
            string key = configuration["Jwt:Key"]
                ?? throw new ArgumentException("The key not found");
            
            byte[] secretKey = Encoding.UTF8.GetBytes(key);

            SymmetricSecurityKey securityKey = new(secretKey);

            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims =
            [
                new(JwtRegisteredClaimNames.Sub, client.Id.ToString()),
                new(JwtRegisteredClaimNames.Name, client.FirstName),
                new(JwtRegisteredClaimNames.Email, client.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            foreach (var role in client.ClientRole!)
                claims.Add(new Claim(ClaimTypes.Role, role.Role!.Name!));

            JwtSecurityToken token = new
            (
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                claims: claims,
                signingCredentials: credentials
            );

            JwtSecurityTokenHandler handler = new();

            return handler.WriteToken(token);
        }
    }
}