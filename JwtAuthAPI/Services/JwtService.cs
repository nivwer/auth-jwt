using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuthAPI.Dtos;
using JwtAuthAPI.Models;
using JwtAuthAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthAPI.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration configuration)
    {
        _config = configuration;
    }

    public string CreateToken(User user)
    {
        var jwt = _config.GetSection("Jwt").Get<JwtDto>();

        if (jwt == null)
        {
            string message = "No valid configurations found for Jwt.";
            throw new ApplicationException(message);
        }

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("id", user.Id.ToString()),
                }
            ),

            Expires = DateTime.UtcNow.AddMinutes(5),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey)),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public int ValidateToken(ClaimsIdentity identity)
    {
        if (!identity.Claims.Any())
        {
            string message = "The identity has no claims.";
            throw new InvalidOperationException(message);
        }

        var id = identity.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

        if (id == null)
        {
            string message = "The 'id' claim is not present in the identity or is null.";
            throw new InvalidOperationException(message);
        }

        return Int32.Parse(id);
    }
}
