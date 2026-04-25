using Microsoft.EntityFrameworkCore;
using Scheduler.API.Common;
using Scheduler.API.Data;
using Scheduler.API.DTOs;
using Scheduler.API.Models;

namespace Scheduler.API.Services;

public class AuthServiceHandler(AppDbContext db, ILogger<AuthServiceHandler> logger)
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

    public async Task<Result<LoginResponse>> Login(LoginRequest loginData)
    {
        var result = new Result<LoginResponse>();
        try
        {
            logger.LogInformation("@{data}", loginData);
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