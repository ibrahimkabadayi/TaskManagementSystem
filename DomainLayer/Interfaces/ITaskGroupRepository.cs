using DomainLayer.Entities;
using Task = DomainLayer.Entities.Task;

namespace DomainLayer.Interfaces;

public interface ITaskGroupRepository: IRepository<TaskGroup>
{
    Task<TaskGroup> GetAllTasksAsync(int id);
}