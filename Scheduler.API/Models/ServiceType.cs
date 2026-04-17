namespace Scheduler.API.Models;

public class ServiceType
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public DateTime DateTimeCreated { get; set; }
    public DateTime? DateTimeUpdated { get; set; }
}