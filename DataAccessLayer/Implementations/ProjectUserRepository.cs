using DataAccessLayer.Context;
using DomainLayer.Entities;
using DomainLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementations;

public class ProjectUserRepository : Repository<ProjectUser>, IProjectUserRepository
{
    public ProjectUserRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<List<ProjectUser>> GetProjectUsersWithDetailsAsync(int projectId)
    {
        return await _context.ProjectUsers
            .Include(pu => pu.User)
            .Include(pu => pu.Project)
            .Where(pu => pu.ProjectId == projectId)
            .ToListAsync();
    }
}