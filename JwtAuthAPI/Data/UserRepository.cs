using JwtAuthAPI.Data.Interfaces;
using JwtAuthAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthAPI.Data;

public class UserRepository(DataContext context) : IUserRepository
{
    private readonly DataContext _context = context;

    public async Task<User?> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        return user;
    }

    public async Task<bool> UserExists(string Username)
    {
        return await _context.Users.AnyAsync(x => x.Username == Username);
    }
}
