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
    private IConfiguration _config;

    public JwtService(IConfiguration configuration)
    {
        _config = configuration;
    }

    public JwtSecurityToken CreateToken(User user)
    {
        var jwt = _config.GetSection("Jwt").Get<JwtDto>();

        if (jwt == null)
        {
            throw new ApplicationException("No valid configurations found for Jwt.");
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("id", user.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
        var login = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            jwt.Issuer,
            jwt.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: login
        );

        return token;
    }
}
