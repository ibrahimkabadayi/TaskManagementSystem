
using DataAccessLayer.Entities;
using Task = DataAccessLayer.Entities.Task;

namespace DataAccessLayer.Repositories.Interfaces;

public interface ITaskRepository : IRepository<Task>
{
    Task<IEnumerable<Task>> GetTasksByProjectAsync(int projectId);
    Task<IEnumerable<Task>> GetTasksByUserAsync(int userId);
    Task<IEnumerable<Task>> GetPendingTasksAsync();
    Task<IEnumerable<Task>> GetCompletedTasksAsync(int projectId);
}