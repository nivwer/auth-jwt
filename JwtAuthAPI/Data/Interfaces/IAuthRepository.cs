using JwtAuthAPI.Dtos;
using JwtAuthAPI.Models;

namespace JwtAuthAPI.Data.Interfaces;
public interface IAuthRepository
{
    Task<User> Register(UserRegisterDto model);
    Task<User?> Login(UserLoginDto model);
    Task<bool> UserExists(string username);
}