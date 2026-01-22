using DomainLayer.Entities;

namespace DomainLayer.Interfaces;

public interface ISectionRepository : IRepository<Section>
{
    Task<Section?> GetSectionWithTasksAsync(int sectionId);
    Task<List<Section>> GetSectionsWithTaskGroupsAsync(int projectId);
}