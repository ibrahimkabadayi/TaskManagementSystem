using Application.DTOs;

namespace Application.Interfaces;

public interface ISectionService
{
    Task<SectionDto?> GetSectionByIdAsync(int id);
    Task<List<SectionDto>> GetAllSectionsAsync();
    Task<string> ChangeBackgroundUrl(int sectionId, string url);
    Task<SectionDto?> GetSectionWithTasksAsync(int sectionId);
    Task<List<SectionDto>> GetSectionsByProjectAsync(int projectId);
}