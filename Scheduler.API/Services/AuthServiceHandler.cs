using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scheduler.API.Common;
using Scheduler.API.Data;
using Scheduler.API.DTOs;
using Scheduler.API.Models;

namespace Scheduler.API.Services;

public class AuthServiceHandler(AppDbContext db, ILogger<AuthServiceHandler> logger, IConfiguration config)
{

    public async Task<Result<object>> Register(RegisterRequest registerData)
    {
        var result = new Result<object>();
        try
        {

            var roleExists = await db.Roles.AnyAsync(r => r.Id == registerData.RoleId);
            if (!roleExists)
            {
                result.Message = "Invalid role id";
                result.ErrorCode = (int)ErrorType.NotFound;
                return result;
            }


            var branchExists = await db.Branches.AnyAsync(r => r.Id == registerData.BranchId);
            if (!branchExists)
            {
                result.Message = "Invalid branch id";
                result.ErrorCode = (int)ErrorType.NotFound;
                return result;
            }


            var email = registerData.Email.ToLower();
            var exists = await db.AdminUsers.AnyAsync(u => u.Email == email);

            if (exists)
            {
                result.Message = "Email address already exists !";
                result.ErrorCode = (int)ErrorType.Conflict;
                return result;
            }

            var newAdminUser = new AdminUser
            {
                RoleId = registerData.RoleId,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerData.Password),
                FullName = registerData.FullName,
                Email = email,
                MobileNum = null,
                DateTimeCreated = DateTime.Now
            };

            await db.AdminUsers.AddAsync(newAdminUser);
            await db.SaveChangesAsync();

            result.IsSucces = true;
            result.Message = "User successfully registered !";

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            result.Message = "Something went wrong!";
            return result;
        }
    }

    private string GenerateToken(AdminUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.RoleId.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<Result<LoginResponse>> Login(LoginRequest loginData)
    {
        var result = new Result<LoginResponse>();
        try
        {

            var email = loginData.Email.ToLower();
            var user = await db.AdminUsers
                .Include(u => u.Role)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                result.ErrorCode = (int)ErrorType.Unauthorized;
                result.Message = "Invalid email or password !";
                return result;
            }

            logger.LogInformation("{@user}", user);

            if (!BCrypt.Net.BCrypt.Verify(loginData.Password, user.PasswordHash))
            {
                result.ErrorCode = (int)ErrorType.Unauthorized;
                result.Message = "Invalid email or password !";
                return result;
            }

            result.IsSucces = true;
            result.Message = "Login success !";
            result.Data = new LoginResponse
            (
                GenerateToken(user),
                user.FullName,
                user.Email,
                user.Role?.Title ?? string.Empty
            );
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            result.Message = "Something went wrong!";
            return result;
        }
    }
}