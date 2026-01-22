using DomainLayer.Entities;

namespace DomainLayer.Interfaces;

public interface IProjectRepository : IRepository<Project>
{
    Task<IEnumerable<Project>> GetProjectsByUserAsync(int userId);
    Task<IEnumerable<Project>> GetActiveProjectsAsync();
    Task<User> GetProjectLeaderAsync(int projectId);
    Task<Project?> GetProjectWithSectionAsync(int projectId);
}