namespace Scheduler.API.Models;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Duration { get; set; }

    public int BranchId { get; set; }
    public required Branch Branch { get; set; }

    public int ServiceTypeId { get; set; }
    public required ServiceType ServiceType { get; set; }

    public DateTime DateTimeCreated { get; set; }
    public DateTime DateTimeUpdated { get; set; }
}