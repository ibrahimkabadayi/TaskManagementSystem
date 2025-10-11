using DataAccessLayer.Context;
using DataAccessLayer.Enums;
using DataAccessLayer.Repositories.Implemantations;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = DataAccessLayer.Entities.Task;

namespace DataAccessLayer.Repositories.Implementations;

public class TaskRepository : Repository<Task>, ITaskRepository
{
    protected TaskRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Task>> GetTasksByProjectAsync(int projectId)
    {
        return await _context.Tasks
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.AssignedTo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetTasksByUserAsync(int userId)
    {
        return await _context.Tasks.AsQueryable()
            .Where(t => t.AssignedToId == userId)
            .Include(t => t.Project)
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetPendingTasksAsync()
    {
        return await _context.Tasks
            .Where(t => t.State == TaskState.Todo || t.State == TaskState.InProgress)
            .Include(t => t.Project)
            .Include(t => t.AssignedTo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetCompletedTasksAsync(int projectId)
    {
        return await _context.Tasks
            .Where(t => t.ProjectId == projectId && t.State == TaskState.Done)
            .ToListAsync();
    }
}