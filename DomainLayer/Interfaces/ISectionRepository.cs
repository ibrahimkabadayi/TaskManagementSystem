using DomainLayer.Entities;

namespace DomainLayer.Interfaces;

public interface ISectionRepository : IRepository<Section>
{
    Task<Section?> GetSectionWithTasksAsync(int sectionId);
}