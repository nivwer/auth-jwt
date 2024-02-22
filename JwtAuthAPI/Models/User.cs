namespace JwtAuthAPI.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public required DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}
