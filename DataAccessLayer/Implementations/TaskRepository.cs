using DataAccessLayer.Context;
using DomainLayer.Enums;
using DomainLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = DomainLayer.Entities.Task;

namespace DataAccessLayer.Implementations;
public class TaskRepository : Repository<Task>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Task>> GetTasksBySectionAsync(int taskGroupId)
    {
        return await _context.Tasks
            .Where(t => t.TaskGroupId == taskGroupId)
            .Include(t => t.AssignedTo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetTasksByUserAsync(int userId)
    {
        return await _context.Tasks.AsQueryable()
            .Where(t => t.AssignedToId == userId)
            .Include(t => t.TaskGroupId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetPendingTasksAsync()
    {
        return await _context.Tasks
            .Where(t => t.State == TaskState.Todo || t.State == TaskState.InProgress)
            .Include(t => t.TaskGroupId)
            .Include(t => t.AssignedTo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetCompletedTasksAsync(int sectionId)
    {
        return await _context.Tasks
            .Where(t => t.TaskGroupId == sectionId && t.State == TaskState.Done)
            .ToListAsync();
    }

    public async Task<Task> GetTaskWithDetailsAsync(int taskId)
    {
        return await _context.Tasks
            .Include(t => t.CreatedBy)
            .ThenInclude(t => t.User)
            .Include(t => t.AssignedTo)
            .ThenInclude(t => t.User)
            .Include(t => t.FinishedBy)
            .ThenInclude(t => t.User)
            .Include(t => t.TaskGroup)
            .Where(t => t.Id == taskId)
            .FirstOrDefaultAsync();
    }
}