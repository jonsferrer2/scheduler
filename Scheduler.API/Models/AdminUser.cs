namespace Scheduler.API.Models;

public class AdminUser
{
    public int Id { get; set; }

    public int RoleId { get; set; }
    public required Role Role { get; set; }

    public string Username { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string MobileNum { get; set; } = "";

    public ICollection<Branch>? Branches { get; set; }

    public DateTime DateTimeCreated { get; set; }
    public DateTime DateTimeUpdated { get; set; }
}