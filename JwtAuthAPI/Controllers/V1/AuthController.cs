using System.IdentityModel.Tokens.Jwt;
using JwtAuthAPI.Data.Interfaces;
using JwtAuthAPI.Dtos;
using JwtAuthAPI.Models;
using JwtAuthAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthAPI.Controllers.V1;

[Route("api/")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthRepository _repo;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwt;

    public AuthController(
        ILogger<AuthController> logger,
        IAuthRepository authRepository,
        IUserRepository userRepository,
        IJwtService jwt
    )
    {
        _logger = logger;
        _repo = authRepository;
        _userRepository = userRepository;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] UserRegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _userRepository.UserExists(model.Username))
        {
            string message = "Username is already taken";
            return BadRequest(message);
        }

        User user = await _repo.Register(model);

        if (user == null)
        {
            string message = "Error occurred during user registration.";
            return BadRequest(message);
        }

        var token = _jwt.CreateToken(user);

        return Ok(new { Token = token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        User? user = await _repo.Login(model);

        if (user == null)
        {
            string message = "Invalid username or password";
            return Unauthorized(message);
        }

        var token = _jwt.CreateToken(user);

        return Ok(new { Token = token });
    }
}
