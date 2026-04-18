using Scheduler.API.Common;
using Scheduler.API.Data;
using Scheduler.API.DTOs;

namespace Scheduler.API.Services;

public class AuthServiceHandler(AppDbContext db, ILogger<AuthServiceHandler> logger)
{

    public async Task<Result<object>> Register(RegisterRequest registerData)
    {
        var result = new Result<object>();
        try
        {
            logger.LogInformation("@{data}", registerData);
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