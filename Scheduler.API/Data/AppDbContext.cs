using Microsoft.EntityFrameworkCore;
using Scheduler.API.Models;

namespace Scheduler.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppointmentType> AppointmentTypes { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<AdminUser> AdminUsers { get; set; }
    public DbSet<ServiceType> ServiceTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.ToTable("branches");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(a => a.BranchName)
                .HasColumnName("branch_name")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(a => a.Address)
                .HasColumnName("address")
                .HasMaxLength(300)
                .IsRequired();

            entity.Property(a => a.DateTimeCreated)
                .HasColumnName("datetime_created")
                .ValueGeneratedNever();

            entity.Property(a => a.DateTimeUpdated)
                .HasColumnName("datetime_updated")
                .ValueGeneratedNever();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(a => a.Title)
                .HasColumnName("title")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(a => a.DateTimeCreated)
                .HasColumnName("datetime_created")
                .ValueGeneratedNever();

            entity.Property(a => a.DateTimeUpdated)
                .HasColumnName("datetime_updated")
                .ValueGeneratedNever();
        });

        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.ToTable("admin_users");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(a => a.RoleId)
                .HasColumnName("role_id")
                .IsRequired();

            entity.Property(a => a.Username)
                .HasColumnName("username")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(a => a.FullName)
                .HasColumnName("fullname")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(a => a.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(a => a.MobileNum)
                .HasColumnName("mobile_num")
                .HasMaxLength(30)
                .IsRequired();

            entity.HasIndex(a => a.Username).IsUnique();
            entity.HasIndex(a => a.Email).IsUnique();
            entity.HasIndex(a => a.MobileNum).IsUnique();

            entity.HasOne(a => a.Role)
                .WithMany()
                .HasForeignKey(a => a.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(a => a.Branches)
                .WithMany(b => b.AdminUsers)
                .UsingEntity(j => j.ToTable("admin_user_branch"));
        });

        modelBuilder.Entity<ServiceType>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(a => a.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(a => a.DateTimeCreated)
                .HasColumnName("datetime_created")
                .ValueGeneratedNever();

            entity.Property(a => a.DateTimeUpdated)
                .HasColumnName("datetime_updated")
                .ValueGeneratedNever();
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(a => a.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(a => a.Duration)
                .HasColumnName("duration")
                .IsRequired();

            entity.Property(a => a.BranchId)
                .HasColumnName("branch_id")
                .IsRequired();

            entity.Property(a => a.ServiceTypeId)
                .HasColumnName("service_type_id")
                .IsRequired();

            entity.Property(a => a.DateTimeCreated)
                .HasColumnName("datetime_created")
                .ValueGeneratedNever();

            entity.Property(a => a.DateTimeUpdated)
                .HasColumnName("datetime_updated")
                .ValueGeneratedNever();

            entity.HasOne(a => a.Branch)
                .WithMany();
        });
    }

}