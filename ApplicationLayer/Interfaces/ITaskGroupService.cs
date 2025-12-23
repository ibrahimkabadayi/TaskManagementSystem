using DataAccessLayer.Entities;

namespace Application.Interfaces;

public interface ITaskGroupService
{
    Task<TaskGroup?> GetSectionByIdAsync(int id);
    Task<List<TaskGroup>> GetAllSectionsAsync();
}