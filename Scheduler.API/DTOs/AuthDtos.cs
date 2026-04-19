using System.ComponentModel.DataAnnotations;
using Scheduler.API.DTOs.Validations;

namespace Scheduler.API.DTOs;

public record LoginRequest(
    [Required]
    string Username,

    [Required]
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
    [StringLength(100, MinimumLength = 3)]
    string Username,

    [Required]
    [StringLength(255, MinimumLength = 3)]
    string Password,

    [Required]
    [StringLength(255, MinimumLength = 3)]
    string FullName,

    [Required]
    [StringLength(255)]
    [EmailAddress]
    string Email,

    [Required]
    [StringLength(255, MinimumLength = 3)]
    [StartsWith(["09", "63", "9"])]
    string MobileNum,

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Value cannot be zero")]
    int RoleId,

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Value cannot be zero")]
    int BranchId
);