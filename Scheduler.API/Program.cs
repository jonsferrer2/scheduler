using Microsoft.EntityFrameworkCore;
using Scheduler.API.Data;
using Scheduler.API.Models;
using Scheduler.API.Services;
using Serilog;
using Serilog.Formatting.Compact;

Log.Logger = new LoggerConfiguration()
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
    builder.Services.AddScoped<AppointmentTypeServiceHandler>();

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
