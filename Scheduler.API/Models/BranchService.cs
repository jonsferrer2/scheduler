namespace Scheduler.API.Models;

public class BranchService
{
    public int BranchId { get; set; }
    public required Branch Branch { get; set; }
    public int ServiceId { get; set; }
    public required Service Service { get; set; }
}