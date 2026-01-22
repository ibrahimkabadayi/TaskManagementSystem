using DomainLayer.Entities;

namespace DomainLayer.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetUsersByProjectAsync(int projectId);
    Task<IEnumerable<Project>> GetUserProjectsAsync(int userId);
}