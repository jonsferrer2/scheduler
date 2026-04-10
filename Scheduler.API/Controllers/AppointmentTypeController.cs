using Microsoft.AspNetCore.Mvc;
using Scheduler.API.DTOs;
using Scheduler.API.Services;

namespace Scheduler.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentTypeController(AppointmentTypeService service) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var res = await service.GetAllAsync();
        return Ok(res);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetByIdAsync(Guid Id)
    {
        var res = await service.GetByIdAsync(Id);
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAppointmentTypeRequest data)
    {
        var res = await service.Create(data);
        return Ok(res);
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> Update(Guid Id, [FromBody] UpdateAppointmentTypeRequest fields)
    {
        var res = await service.Update(Id, fields);
        if (res.Message.Contains("not found"))
        {
            return NotFound(res);
        }

        return Ok(res);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(Guid Id)
    {
        var res = await service.Delete(Id);
        return Ok(res);
    }

}