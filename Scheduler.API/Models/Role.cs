namespace Scheduler.API.Models;

public class Role
{
    public int Id { get; set; }
    public string Title { get; set; } = "";

    public DateTime DateTimeCreated { get; set; }
    public DateTime? DateTimeUpdated { get; set; }
}