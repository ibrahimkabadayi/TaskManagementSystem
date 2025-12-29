using DataAccessLayer.Context;
using DomainLayer.Entities;
using DomainLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementations;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
        
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        
        return user;
    }

    public async Task<IEnumerable<User>> GetUsersByProjectAsync(int projectId)
    {
        return await _context.ProjectUsers
            .Where(pu => pu.ProjectId == projectId)
            .Include(pu => pu.User)
            .Select(pu => pu.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetUserProjectsAsync(int userId)
    {
        return await _context.ProjectUsers
            .Where(pu => pu.UserId == userId)
            .Include(pu => pu.Project)
            .Select(pu => pu.Project)
            .ToListAsync();
    }
}