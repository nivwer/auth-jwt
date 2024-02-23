using System.Security.Claims;
using JwtAuthAPI.Data.Interfaces;
using JwtAuthAPI.Dtos;
using JwtAuthAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthAPI.Controllers.V1;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IUserRepository _repo;
    private readonly IJwtService _jwt;

    public UserController(
        ILogger<AuthController> logger,
        IUserRepository userRepository,
        IJwtService jwt
    )
    {
        _logger = logger;
        _repo = userRepository;
        _jwt = jwt;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        if (identity == null)
        {
            string message = "Token not provided in the request.";
            return Unauthorized(message);
        }

        int id;

        try
        {
            id = _jwt.ValidateToken(identity);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }

        var user = await _repo.GetUser(id);

        if (user == null)
        {
            string message = "User associated with the token not found.";
            return Unauthorized(message);
        }

        return Ok(
            new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive,
            }
        );
    }
}
