using Application.DTOs;

namespace Application.Interfaces;

public interface ISectionService
{
    Task<SectionDto?> GetSectionByIdAsync(int id);
    Task<List<SectionDto>> GetAllSectionsAsync();
    Task<string> ChangeBackgroundUrl(int sectionId, string url);
    Task<SectionDto?> GetSectionWithTasksAsync(int sectionId);
    Task<List<SectionDto>> GetSectionsByProjectAsync(int projectId);
    Task<SectionDto> CreateSectionAsync(int projectId, string sectionName, string backgroundUrl);
    Task<bool> DeleteSectionAsync(int sectionId);
}