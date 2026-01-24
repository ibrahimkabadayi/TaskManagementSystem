using Application.DTOs;

namespace Application.Interfaces;

public interface ITaskService
{
    Task<TaskDto?> GetTaskByIdAsync(int id);
    Task<List<TaskDto>> GetAllTasksAsync(string email);
    Task<TaskDetailsDto> GetTaskDetailsAsync(int id);
    Task<TaskDto> SaveTaskAsync(int userId, string taskTitle, string taskGroupName, int sectionId);
    Task<int> ChangeTaskGroup(int taskId, int taskGroupId, int newPosition);
    Task<int> ChangeTaskPriority(int taskId, string priority);
    Task<string> DeleteTask(int taskId, int userId, int projectId);
    Task<int> ChangeTaskState(int taskId, int taskState, int userId, int projectId);
    Task<int> ChangeTaskDescription(int userId, int taskId, int projectId, string description);
    Task<int> ChangeTaskDueDate(int userId, int taskId, int projectId, string dueDate);
    Task<int> AssignUserToTask(int userId, int taskId, int projectId, string userEmail);
    Task<int> UpdateTitle(int taskId, string title, int userId, int projectId);
}