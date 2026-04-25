using Microsoft.EntityFrameworkCore;
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

    var app = builder.Build();

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
