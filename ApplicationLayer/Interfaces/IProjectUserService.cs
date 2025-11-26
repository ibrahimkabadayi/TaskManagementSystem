using Application.DTOs;

namespace Application.Interfaces;

public interface IProjectUserService
{
    Task<ProjectUserDto?> GetProjectByIdAsync(int id);
    Task<List<ProjectUserDto?>> GetAllProjectsAsync();
}