using DomainLayer.Entities;
using Task = System.Threading.Tasks.Task;

namespace DomainLayer.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetUsersByProjectAsync(int projectId);
    Task<IEnumerable<Project>> GetUserProjectsAsync(int userId);
    Task<User> GetUserWithProjectUsersAsync(int id);
}