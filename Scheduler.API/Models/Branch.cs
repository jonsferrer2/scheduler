namespace Scheduler.API.Models;

public class Branch
{
    public int Id { get; set; }
    public string BranchName { get; set; } = "";
    public string Address { get; set; } = "";

    // public ICollection<BranchService> BranchServices { get; set; } = [];

    public DateTime DateTimeCreated { get; set; }
    public DateTime? DateTimeUpdated { get; set; }
}