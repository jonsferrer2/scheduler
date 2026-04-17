namespace Scheduler.API.Models;

public class AppointmentType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public int Duration { get; set; }
    public int TransactionType { get; set; }
    public DateTime DateTimeCreated { get; set; }
    public DateTime? DateTimeUpdated { get; set; }
}