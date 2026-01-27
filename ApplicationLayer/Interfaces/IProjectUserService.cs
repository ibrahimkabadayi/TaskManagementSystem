using Application.DTOs;

namespace Application.Interfaces;

public interface IProjectUserService
{
    Task<ProjectUserDto?> GetProjectUserByIdAsync(int id);
    Task<List<ProjectUserDto?>> GetAllProjectUsersOfOneProjectAsync(int projectId);
    Task<List<ProjectUserDetailsDto>> GetAllProjectUserDetailsOfOneProjectAsync(int projectId);
    Task<int> ChangeRole(int projectUserId, string newRole, int userId, int projectId);
    Task<int> RemoveProjectUser(int projectUserId, int userId, int projectId);
}