using DomainLayer.Entities;

namespace DomainLayer.Interfaces;

public interface IProjectUserRepository : IRepository<ProjectUser>
{
    Task<List<ProjectUser>> GetProjectUsersWithDetailsAsync(int projectId);
}