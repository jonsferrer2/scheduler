using Microsoft.EntityFrameworkCore;
using Scheduler.API.Models;

namespace Scheduler.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<AppointmentType> AppointmentTypes => Set<AppointmentType>();
}