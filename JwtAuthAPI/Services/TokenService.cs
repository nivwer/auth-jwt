// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using JwtAuthAPI.Models;
// using JwtAuthAPI.Services.Interfaces;
// using Microsoft.Extensions.Configuration;
// using Microsoft.IdentityModel.Tokens;

// namespace JwtAuthAPI.Services;
// public class TokenService(IConfiguration config) : ITokenService
// {
//     private readonly SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(config["Jwt:Key"]));

//     string ITokenService.CreateToken(User user)
//     {
//         var claims = new List<Claim>{
//                 new(ClaimTypes.NameIdentifier, user.Username),
//             };

//         var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

//         var tokenDescriptor = new SecurityTokenDescriptor
//         {
//             Subject = new ClaimsIdentity(claims),
//             Expires = DateTime.Now.AddDays(2),
//             SigningCredentials = credentials
//         };

//         var tokenHandler = new JwtSecurityTokenHandler();
//         var token = tokenHandler.CreateToken(tokenDescriptor);
//         return tokenHandler.WriteToken(token);


//     }
// }