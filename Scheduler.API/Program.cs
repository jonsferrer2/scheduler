using Microsoft.EntityFrameworkCore;
using Scheduler.API.Data;
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

    builder.Services.AddScoped<BranchService>();
    builder.Services.AddScoped<AppointmentTypeService>();

    var app = builder.Build();

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Message - {ex.Message}");
}
