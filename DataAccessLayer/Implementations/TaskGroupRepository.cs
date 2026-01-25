using DataAccessLayer.Context;
using DomainLayer.Entities;
using DomainLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = DomainLayer.Entities.Task;

namespace DataAccessLayer.Implementations;

public class TaskGroupRepository : Repository<TaskGroup>, ITaskGroupRepository
{
    public TaskGroupRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<TaskGroup> GetAllTasksAsync(int id)
    {
        return await _context.TaskGroups
            .Include(x => x.Tasks)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();
    }
}