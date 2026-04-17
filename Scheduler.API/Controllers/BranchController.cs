using Microsoft.AspNetCore.Mvc;
using Scheduler.API.DTOs;
using Scheduler.API.Services;

namespace Scheduler.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BranchController(BranchServiceHandler service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var res = await service.GetAllAsync();
        return Ok(res);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetByIdAsync(int Id)
    {
        var res = await service.GetByIdAsync(Id);
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBranch(CreateBranchRequest data)
    {
        var res = await service.Create(data);
        return Ok(res);
    }
}