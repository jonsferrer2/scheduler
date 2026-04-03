using Microsoft.EntityFrameworkCore;
using Scheduler.API.Common;
using Scheduler.API.Data;
using Scheduler.API.DTOs;
using Scheduler.API.Models;

namespace Scheduler.API.Services;

public class AppointmentTypeService(AppDbContext db, ILogger<AppointmentTypeService> logger)
{
    public async Task<Result<List<AppointmentTypeDto>>> GetAllAsync()
    {
        var result = new Result<List<AppointmentTypeDto>>();
        try
        {
            var appointmentTypes = await db.AppointmentTypes.ToListAsync();
            result.IsSucces = true;
            result.Message = "Appointment types fetched successfully!";
            result.Data = appointmentTypes.Select(i => new AppointmentTypeDto(i.Id, i.Name, i.Duration, i.TransactionType)).ToList();

            logger.LogInformation("Appointment types list : {@result}", result);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            result.Message = "Something went wrong!";
            return result;
        }
    }
    public async Task<Result<AppointmentTypeDto>> GetByIdAsync(Guid Id)
    {
        var result = new Result<AppointmentTypeDto>();
        try
        {
            var appointmentType = await db.AppointmentTypes.FindAsync(Id);

            result.IsSucces = true;
            if (appointmentType is null)
            {
                result.Message = "Appointment type not found!";
                return result;
            }

            result.Message = "Appointment type fetch sucessfully!";
            result.Data = new AppointmentTypeDto(appointmentType.Id, appointmentType.Name, appointmentType.Duration, appointmentType.TransactionType);
            return result;

        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            result.Message = "Something went wrong!";
            return result;
        }
    }

    public async Task<Result<CreateAppointmentTypeResponse>> Create(CreateAppointmentTypeRequest appointmentType)
    {
        bool exists = await db.AppointmentTypes.AnyAsync(i => i.Name.Trim().ToLower() == appointmentType.Name.Trim().ToLower()
            && i.TransactionType == appointmentType.TransactionType
        );

        var result = new Result<CreateAppointmentTypeResponse>();

        if (exists)
        {
            result.Message = "Appointment type name already exists!";
            return result;
        }

        var newAppointmentType = new AppointmentType
        {
            Name = appointmentType.Name,
            Duration = appointmentType.Duration,
            TransactionType = appointmentType.TransactionType,
            DateTimeCreated = DateTime.UtcNow,
            DateTimeUpdated = DateTime.UtcNow,
        };

        try
        {
            await db.AppointmentTypes.AddAsync(newAppointmentType);
            await db.SaveChangesAsync();

            result.IsSucces = true;
            result.Message = "Appointment type saved successfully!";
            result.Data = new CreateAppointmentTypeResponse(newAppointmentType.Id, newAppointmentType.Name, newAppointmentType.Duration, newAppointmentType.TransactionType);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            result.Message = "Something went wrong!";
            return result;
        }

    }
}