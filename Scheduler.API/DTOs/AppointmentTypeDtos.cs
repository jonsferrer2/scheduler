using System.ComponentModel.DataAnnotations;

namespace Scheduler.API.DTOs;

public record AppointmentTypeDto(Guid Id, string Name, int Duration, int TransactionType);
public record CreateAppointmentTypeRequest(
    [Required]
    [StringLength(100, MinimumLength = 3)]
    string Name,

    [Required]
    [Range(5, 100)]
    int Duration,

    [Required]
    [Range(1,2)]
    int TransactionType
);
public record CreateAppointmentTypeResponse(Guid Id, string Name, int Duration, int TransactionType);