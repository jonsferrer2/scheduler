using System.ComponentModel.DataAnnotations;

namespace Scheduler.API.DTOs;

public record BranchDto(int Id, string BranchName, string Address);
public record CreateBranchRequest(
    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Branch name should be 3-100 characters.")]
    string BranchName,

    [Required]
    [StringLength(300, MinimumLength = 3, ErrorMessage = "Branch name should be 3-300 characters.")]
    string Address
);
public record CreateBranchResponse(int Id, string BranchName, string Address);