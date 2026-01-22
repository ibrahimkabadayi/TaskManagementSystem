using DataAccessLayer.Context;
using DataAccessLayer.Implementations;
using DomainLayer.Entities;
using DomainLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = DomainLayer.Entities.Task;

namespace DataAccessLayer.Implementations;

public class SectionRepository : Repository<Section>, ISectionRepository
{
    public SectionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Section?> GetSectionWithTasksAsync(int sectionId)
    {
        return await _context.Sections
            .Include(s => s.TaskGroups)!
            .ThenInclude(tg => tg.Tasks)!
            .ThenInclude(t => t.CreatedBy)
            .ThenInclude(pu => pu.User) 

            .Include(s => s.TaskGroups)!
            .ThenInclude(tg => tg.Tasks)!
            .ThenInclude(t => t.AssignedTo)
            .ThenInclude(pu => pu!.User)
        
            .Where(s => s.Id == sectionId)
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<Section>> GetSectionsWithTaskGroupsAsync(int projectId)
    {
        return await _context.Sections
            .Include(s => s.TaskGroups)
                .ThenInclude(t => t.Tasks)
                    .ThenInclude(t => t.CreatedBy)
            .Where(s => s.ProjectId == projectId)
            .ToListAsync();
    }
}