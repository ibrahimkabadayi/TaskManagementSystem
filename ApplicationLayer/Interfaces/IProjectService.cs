using Application.DTOs;

namespace Application.Interfaces;

public interface IProjectService
{
    Task<ProjectDto?> GetProjectByIdAsync(int id);
    Task<List<ProjectDto?>> GetAllProjectsAsync();
}