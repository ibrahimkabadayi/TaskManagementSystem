using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IProjectRepository : IRepository<Project>
{
    Task<IEnumerable<Project>> GetProjectsByUserAsync(int userId);
    Task<IEnumerable<Project>> GetActiveProjectsAsync();
    Task<User> GetProjectLeaderAsync(int projectId);
}