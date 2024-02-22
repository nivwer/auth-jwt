using System.IdentityModel.Tokens.Jwt;
using JwtAuthAPI.Data.Interfaces;
using JwtAuthAPI.Dtos;
using JwtAuthAPI.Models;
using JwtAuthAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthAPI.Controllers.V1;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthRepository _repo;
    private readonly IJwtService _jwt;

    public AuthController(
        ILogger<AuthController> logger,
        IAuthRepository authRepository,
        IJwtService jwt
    )
    {
        _logger = logger;
        _repo = authRepository;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] UserRegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _repo.UserExists(model.Username))
        {
            return BadRequest("Username is already taken");
        }

        User user = await _repo.Register(model);

        return Created();
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
            return Unauthorized("Invalid username or password");
        }

        // Generar y devolver token JWT aquí...

        var token = _jwt.CreateToken(user);

        return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}
