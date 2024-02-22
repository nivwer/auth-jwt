using JwtAuthAPI.Data.Interfaces;
using JwtAuthAPI.Dtos;
using JwtAuthAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthAPI.Controllers.V1;

[Route("api/auth")]
[ApiController]
public class AuthController(ILogger<AuthController> logger, IAuthRepository authRepository)
    : ControllerBase
{
    private readonly ILogger<AuthController> _logger = logger;
    private readonly IAuthRepository _repo = authRepository;

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

        // return Ok(new { Token = "your_generated_jwt_token" });

        return Ok(user);
    }
}
