using Microsoft.AspNetCore.Mvc;
using Scheduler.API.DTOs;
using Scheduler.API.Services;
using Serilog;

namespace Scheduler.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthServiceHandler service) : ControllerBase
{
    private bool IsWeb => Request.Headers["X-Client-Type"].ToString() == "web";

    private void AttachTokenToCookie(string token)
    {
        Response.Cookies.Append("auth_token", token, new CookieOptions
        {
            HttpOnly = true,
            // Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(8)
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest data)
    {
        var res = await service.Register(data);
        if (!res.IsSucces)
            return StatusCode(res.ErrorCode, res);

        return Ok(res);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest data)
    {
        var res = await service.Login(data);
        if (!res.IsSucces)
            return StatusCode(res.ErrorCode, res);


        if (IsWeb && res.Data != null && res.Data.Token != null)
        {
            AttachTokenToCookie(res.Data.Token);
            res.Data = res.Data with { Token = null };
        }

        return Ok(res);
    }
}