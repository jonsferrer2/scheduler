using System.ComponentModel.DataAnnotations;
using Scheduler.API.DTOs.Validations;

namespace Scheduler.API.DTOs;

public record LoginRequest(
    [Required]
    [EmailAddress]
    [StringLength(255, MinimumLength = 3)]
    string Email,

    [Required]
    [StringLength(255, MinimumLength = 3)]
    string Password
);

public record LoginResponse(
    string Token,
    string FullName,
    string Email,
    string Role
);
public record RegisterRequest(

    [Required]
    [StringLength(255)]
    [EmailAddress]
    string Email,

    [Required]
    [StringLength(255, MinimumLength = 3)]
    string Password,

    [Required]
    [StringLength(255, MinimumLength = 3)]
    string FullName,

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Value cannot be zero")]
    int RoleId,

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Value cannot be zero")]
    int BranchId
);