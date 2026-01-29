using DataAccessLayer.Context;
using DomainLayer.Entities;
using DomainLayer.Enums;
using DomainLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementations;

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

    public async Task<List<Project>> GetAllProjectsOfOneUserAsync(int userId)
    {
        return await _context.Projects
            .Where(p => p.ProjectUsers.Any(pu => pu.UserId == userId))
            .Include(p => p.Sections)
            .Include(p => p.ProjectUsers)
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

    public async Task<Project?> GetProjectWithSectionAsync(int projectId)
    {
        return await _context.Projects
            .Where(x => x.Id == projectId)
            .Select(x => new Project
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ProjectUsers = x.ProjectUsers!.Select(t => new ProjectUser
                {
                    Id = t.Id,
                    JoinedDate = t.JoinedDate,
                    IsActive = t.IsActive,
                    Project = t.Project,
                    ProjectId = t.ProjectId,
                    Role = t.Role,
                    Title = t.Title,
                    User = t.User,
                    UserId = t.UserId
                }).ToList(),
                Sections = x.Sections.Select(s => new Section
                {
                    Id = s.Id,
                    ImageUrl = s.ImageUrl,
                    Name = s.Name,
                    Project = s.Project,
                    ProjectId = s.ProjectId,
                    TaskGroups = s.TaskGroups!.ToList()
                }).ToList()
            }).FirstOrDefaultAsync();
    }
}