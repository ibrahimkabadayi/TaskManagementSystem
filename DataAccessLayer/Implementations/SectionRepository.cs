using DataAccessLayer.Context;
using DataAccessLayer.Implementations;
using DomainLayer.Entities;
using DomainLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementations;

public class SectionRepository : Repository<Section>, ISectionRepository
{
    public SectionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Section?> GetSectionWithTasksAsync(int sectionId)
    {
        return await _context.Sections.Where(x => x.Id == sectionId)
            .Include(x => x.TaskGroups)
            .FirstOrDefaultAsync();
    }
}