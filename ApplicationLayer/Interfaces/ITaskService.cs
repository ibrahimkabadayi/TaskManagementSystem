using Application.DTOs;

namespace Application.Interfaces;

public interface ITaskService
{
    Task<TaskDto?> GetTaskByIdAsync(int id);
    Task<List<TaskDto>> GetAllTasksAsync(string email);
    Task<TaskDetailsDto> GetTaskDetailsAsync(string taskTitle, string taskGroupName, string sectionName);
    Task<TaskDto> SaveTaskAsync(int userId, string taskTitle, string taskGroupName, string sectionName);
    Task<int> ChangeTaskGroup(int taskId, int taskGroupId, int newPosition);
}