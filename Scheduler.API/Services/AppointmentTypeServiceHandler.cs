using Microsoft.EntityFrameworkCore;
using Scheduler.API.Common;
using Scheduler.API.Data;
using Scheduler.API.DTOs;
using Scheduler.API.Models;

namespace Scheduler.API.Services;

public class AppointmentTypeServiceHandler(AppDbContext db, ILogger<AppointmentTypeServiceHandler> logger)
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
        var result = new Result<CreateAppointmentTypeResponse>();
        try
        {
            bool exists = await db.AppointmentTypes.AnyAsync(i => i.Name.Trim().ToLower() == appointmentType.Name.Trim().ToLower()
                && i.TransactionType == appointmentType.TransactionType
            );

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

    public async Task<Result<CreateAppointmentTypeResponse>> Update(Guid Id, UpdateAppointmentTypeRequest fields)
    {
        var result = new Result<CreateAppointmentTypeResponse>();
        try
        {
            var record = await db.AppointmentTypes.FindAsync(Id);
            if (record == null)
            {
                result.Message = "Appointment type not found!";
                return result;
            }

            record.Name = fields.Name is null ? record.Name : fields.Name;
            record.Duration = fields.Duration ?? record.Duration;
            record.DateTimeUpdated = DateTime.UtcNow;

            await db.SaveChangesAsync();
            result.IsSucces = true;
            result.Message = "Appointment type successfully updated!";
            result.Data = new CreateAppointmentTypeResponse(record.Id, record.Name, record.Duration, record.TransactionType);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            result.Message = "Something went wrong";
            return result;
        }
    }

    public async Task<Result<object>> Delete(Guid Id)
    {
        var result = new Result<object>();
        try
        {
            var res = db.AppointmentTypes
                .Where(i => i.Id == Id)
                .ExecuteDeleteAsync();

            result.IsSucces = true;
            result.Message = "Appointment type successfully deleted!";
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            result.Message = "Something went wrong";
            return result;
        }
    }
}