using JwtAuthAPI.Data.Interfaces;
using JwtAuthAPI.Dtos;
using JwtAuthAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthAPI.Data;

public class AuthRepository(DataContext context) : IAuthRepository
{
    private readonly DataContext _context = context;

    public async Task<User> Register(UserRegisterDto model)
    {
        byte[] passwordSalt = GenerateSalt();
        byte[] passwordHash = CreatePasswordHash(model.Password, passwordSalt);

        var user = new User
        {
            Username = model.Username,
            CreatedAt = model.CreatedAt,
            IsActive = model.IsActive,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> Login(UserLoginDto model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == model.Username);

        if (user == null)
            return null;

        if (!VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
            return null;

        return user;
    }

    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[16];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    private static byte[] CreatePasswordHash(string password, byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
        return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPasswordHash(
        string password,
        byte[] passwordHash,
        byte[] passwordSalt
    )
    {
        byte[] computedHash = CreatePasswordHash(password, passwordSalt);
        return computedHash.SequenceEqual(passwordHash);
    }
}
