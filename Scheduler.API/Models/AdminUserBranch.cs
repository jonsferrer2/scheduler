namespace Scheduler.API.Models;

public class AdminUserBranch
{
    public int AdminUserId { get; set; }
    public required AdminUser AdminUser { get; set; }
    public int BranchId { get; set; }
    public required Branch Branch { get; set; }
}