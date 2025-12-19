using DataAccessLayer.Context;
using DataAccessLayer.Enums;
using DataAccessLayer.Repositories.Implemantations;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = DataAccessLayer.Entities.Task;

namespace DataAccessLayer.Repositories.Implementations;

public class TaskRepository : Repository<Task>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Task>> GetTasksBySectionAsync(int sectionId)
    {
        return await _context.Tasks
            .Where(t => t.SectionId == sectionId)
            .Include(t => t.AssignedTo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetTasksByUserAsync(int userId)
    {
        return await _context.Tasks.AsQueryable()
            .Where(t => t.AssignedToId == userId)
            .Include(t => t.Section)
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetPendingTasksAsync()
    {
        return await _context.Tasks
            .Where(t => t.State == TaskState.Todo || t.State == TaskState.InProgress)
            .Include(t => t.Section)
            .Include(t => t.AssignedTo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetCompletedTasksAsync(int sectionId)
    {
        return await _context.Tasks
            .Where(t => t.SectionId == sectionId && t.State == TaskState.Done)
            .ToListAsync();
    }
}