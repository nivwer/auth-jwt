using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JwtAuthAPI.Models;

namespace JwtAuthAPI.Services.Interfaces;

public interface IJwtService
{
    string CreateToken(User user);
    int ValidateToken(ClaimsIdentity identity);
}
