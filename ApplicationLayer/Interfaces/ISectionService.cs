using Application.DTOs;

namespace Application.Interfaces;

public interface ISectionService
{
    Task<SectionDto?> GetSectionByIdAsync(int id);
    Task<List<SectionDto>> GetAllSectionsAsync();
}