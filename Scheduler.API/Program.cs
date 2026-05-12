using System.Text;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scheduler.API.Data;
using Scheduler.API.Models;
using Scheduler.API.Services;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .WriteTo.File("Logs/TraceLog-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

try
{
    builder.Services.AddControllers();

    var connectionString = builder.Configuration.GetConnectionString("Default");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

    builder.Services.AddScoped<BranchServiceHandler>();
    builder.Services.AddScoped<AuthServiceHandler>();
    // builder.Services.AddScoped<AppointmentTypeServiceHandler>();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var authHeader = context.Request.Headers.Authorization.ToString();

                    if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                    {
                        var rawToken = authHeader.Replace("Bearer ", "");
                        context.Token = rawToken;

                        // var parts = rawToken.Split(".");
                        // var header = parts[0];
                        // var payload = parts[1];
                        // var signature = parts[2];

                        // var decodedHeader = Base64UrlEncoder.Decode(header);
                        // var decodedPayload = Base64UrlEncoder.Decode(payload);

                        // Log.Information("decodedHeader - {@decodedHeader}", decodedHeader);
                        // Log.Information("decodedPayload - {@decodedPayload}", decodedPayload);
                        // Log.Information("signature - {@signature}", signature);
                        return Task.CompletedTask;
                    }

                    var cookieToken = context.Request.Cookies["auth_token"];
                    if (!string.IsNullOrEmpty(cookieToken))
                    {
                        context.Token = cookieToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });

    builder.Services.AddAuthorization();

    var app = builder.Build();

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();


    // Seed DB
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();

        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role
                {
                    Id = 1,
                    Title = "Admin",
                    DateTimeCreated = DateTime.Now,
                },
                new Role
                {
                    Id = 2,
                    Title = "Manager",
                    DateTimeCreated = DateTime.Now,
                },
                new Role
                {
                    Id = 3,
                    Title = "Agent",
                    DateTimeCreated = DateTime.Now,
                }
            );

            await context.SaveChangesAsync();
        }
    }


    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Message - {ex.Message}");
}
