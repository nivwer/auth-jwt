using System.ComponentModel.DataAnnotations;

namespace JwtAuthAPI.Dtos;
public class UserRegisterDto
{
    [Required(ErrorMessage = "This field is required.")]
    [MaxLength(32, ErrorMessage = "Maximum 32 characters.")]
    [MinLength(3, ErrorMessage = "Minimum 3 characters.")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [MaxLength(132, ErrorMessage = "Maximum 132 characters.")]
    [MinLength(8, ErrorMessage = "Minimum 8 characters.")]
    public required string Password { get; set; }

    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public UserRegisterDto()
    {
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }
}
