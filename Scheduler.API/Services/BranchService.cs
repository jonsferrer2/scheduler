using Microsoft.EntityFrameworkCore;
using Scheduler.API.Common;
using Scheduler.API.Data;
using Scheduler.API.DTOs;
using Scheduler.API.Models;

namespace Scheduler.API.Services;

public class BranchService(AppDbContext db, ILogger<BranchService> logger)
{

    public async Task<Result<List<BranchDto>>> GetAllAsync()
    {
        var result = new Result<List<BranchDto>>();
        try
        {
            var branches = await db.Branches.ToListAsync();
            result.IsSucces = true;
            result.Message = "Branches fetched successfully!";
            result.Data = branches.Select(b => new BranchDto(b.Id, b.BranchName, b.Address)).ToList();

            logger.LogInformation("Branches list : {@result}", result);

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            result.Message = "Something went wrong!";
            return result;
        }
    }

    public async Task<Result<BranchDto>> GetByIdAsync(int Id)
    {
        var result = new Result<BranchDto>();
        try
        {
            var branch = await db.Branches.FindAsync(Id);

            result.IsSucces = true;
            if (branch is null)
            {
                result.Message = "Branch not found!";
                return result;
            }

            result.Message = "Branch fetch sucessfully!";
            result.Data = new BranchDto(branch.Id, branch.BranchName, branch.Address);
            return result;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            result.Message = "Something went wrong!";
            return result;
        }
    }

    public async Task<Result<CreateBranchResponse>> Create(CreateBranchRequest branch)
    {
        bool exists = await db.Branches.AnyAsync(b => b.BranchName.Trim().ToLower() == branch.BranchName.Trim().ToLower());

        var result = new Result<CreateBranchResponse>();

        if (exists)
        {
            result.Message = "Branch name already exists!";
            return result;
        }

        var newBranch = new Branch
        {
            BranchName = branch.BranchName,
            Address = branch.Address,
            DateTimeCreated = DateTime.UtcNow,
            DateTimeUpdated = DateTime.UtcNow,
        };

        try
        {
            await db.Branches.AddAsync(newBranch);
            await db.SaveChangesAsync();

            result.IsSucces = true;
            result.Message = "Branch saved successfully!";
            result.Data = new CreateBranchResponse(newBranch.Id, newBranch.BranchName, newBranch.Address);

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            result.Message = "Something went wrong!";
            return result;
        }

    }

}