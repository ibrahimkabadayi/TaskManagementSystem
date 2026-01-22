using Application.DTOs;

namespace Application.Interfaces;

public interface ITaskGroupService
{
    Task<TaskGroupDto?> GetSectionByIdAsync(int id);
    Task<List<TaskGroupDto>> GetAllSectionsAsync();
    Task<TaskGroupDto?> SaveTaskGroupAsync(string taskGroupName, int sectionId, int userId);
    Task<TaskGroupDto> ChangeTaskGroupNameAsync(int id, string newTaskGroupName);
    Task DeleteTaskGroupAsync(int id);
    Task<List<TaskDto>> GetAllTasksAsync(int id);
}