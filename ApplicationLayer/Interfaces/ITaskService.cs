using Application.DTOs;

namespace Application.Interfaces;

public interface ITaskService
{
    Task<TaskDto?> GetTaskByIdAsync(int id);
    Task<List<TaskDto>> GetAllTasksAsync(string email);
}