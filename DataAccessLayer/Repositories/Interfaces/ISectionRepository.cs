using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories.Interfaces;

public interface ISectionRepository : IRepository<Section>
{
    Task<Section?> GetSectionWithTasksAsync(int sectionId);
}