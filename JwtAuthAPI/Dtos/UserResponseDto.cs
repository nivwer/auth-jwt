namespace JwtAuthAPI.Dtos;

public class UserResponseDto
{
    public required int Id { get; set; }
    public required string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}
