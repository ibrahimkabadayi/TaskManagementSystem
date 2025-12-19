using Application.DTOs;

namespace Application.Interfaces;

public interface IProjectUserService
{
    Task<ProjectUserDto?> GetProjectUserByIdAsync(int id);
    Task<List<ProjectUserDto?>> GetAllProjectsAsync();
}