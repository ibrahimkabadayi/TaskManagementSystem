using Task = DomainLayer.Entities.Task;
namespace DomainLayer.Interfaces;

public interface ITaskRepository : IRepository<Task>
{
    Task<IEnumerable<Task>> GetTasksBySectionAsync(int sectionId);
    Task<IEnumerable<Task>> GetTasksByUserAsync(int userId);
    Task<IEnumerable<Task>> GetPendingTasksAsync();
    Task<IEnumerable<Task>> GetCompletedTasksAsync(int sectionId);
}