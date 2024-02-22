using System.IdentityModel.Tokens.Jwt;
using JwtAuthAPI.Models;

namespace JwtAuthAPI.Services.Interfaces;

public interface IJwtService
{
    JwtSecurityToken CreateToken(User user);
}
