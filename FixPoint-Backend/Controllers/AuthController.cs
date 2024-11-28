using FixPoint_Backend.DataAccess;
using FixPoint_Backend.Services.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace FixPoint_Backend.Controllers;

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO loginDto)
    {
        var token = _authService.Login(loginDto);
        if (token == null)
        {
            return Unauthorized("Invalid credentials");
        }
        return Ok(token);
    }
}