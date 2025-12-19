using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Repositories.Implemantations;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations;

public class ProjectRepository : Repository<Project>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Project>> GetProjectsByUserAsync(int userId)
    {
        return await _context.ProjectUsers
            .Where(pu => pu.UserId == userId)
            .Select(pu => pu.Project)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetActiveProjectsAsync()
    {
        return await _context.Projects
            .Where(p => p.EndDate == null || p.EndDate > DateTime.Now)
            .Include(p => p.ProjectUsers)
            .ToListAsync();
    }

    public async Task<User> GetProjectLeaderAsync(int projectId)
    {
        var projectUser = await _context.ProjectUsers
            .Include(pu => pu.User)
            .FirstOrDefaultAsync(pu => 
                pu.ProjectId == projectId && 
                pu.Role == ProjectRole.Leader);

        return projectUser?.User ?? throw new Exception("Project not found");
    }
}