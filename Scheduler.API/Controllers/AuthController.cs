using Microsoft.AspNetCore.Mvc;
using Scheduler.API.DTOs;
using Scheduler.API.Services;

namespace Scheduler.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthServiceHandler service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest data)
    {
        var res = await service.Register(data);
        return Ok(res);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest data)
    {
        var res = await service.Login(data);
        return Ok(res);
    }
}