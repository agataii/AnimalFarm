using AnimalFarm.Application.DTOs.Auth;
using AnimalFarm.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimalFarm.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var message = await _authService.RegisterAsync(dto);
        return Ok(new { message });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }

    [HttpGet("activate")]
    [AllowAnonymous]
    public async Task<IActionResult> Activate([FromQuery] string userId, [FromQuery] string token)
    {
        var result = await _authService.ActivateAsync(new ActivateDto(userId, token));
        if (!result)
            return BadRequest(new { message = "Invalid activation link or token." });

        return Ok(new { message = "Account activated successfully." });
    }
}
