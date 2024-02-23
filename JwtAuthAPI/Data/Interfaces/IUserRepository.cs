using JwtAuthAPI.Models;

namespace JwtAuthAPI.Data.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUser(int id);
    Task<bool> UserExists(string username);
}
