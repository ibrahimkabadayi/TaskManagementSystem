using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Implemantations;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations;

public class SectionRepository : Repository<Section>, ISectionRepository
{
    public SectionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Section?> GetSectionWithTasksAsync(int sectionId)
    {
        return await _context.Sections.Where(x => x.Id == sectionId)
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync();
    }
}