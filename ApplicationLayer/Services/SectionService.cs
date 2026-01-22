using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DomainLayer.Interfaces;

namespace Application.Services;

public class SectionService : ISectionService
{
    private readonly ISectionRepository _sectionRepository;
    private readonly IMapper _mapper;

    public SectionService(ISectionRepository sectionRepository, IMapper mapper)
    {
        _sectionRepository = sectionRepository;
        _mapper = mapper;
    }
    
    public async Task<SectionDto?> GetSectionByIdAsync(int id)
    {
        var section = await _sectionRepository.GetByAsyncId(id);
        return _mapper.Map<SectionDto>(section) ?? null;
    }

    public async Task<List<SectionDto>> GetAllSectionsAsync()
    {
        var allSections = await _sectionRepository.GetAllAsync();
        return _mapper.Map<List<SectionDto>>(allSections)!;
    }

    public async Task<string> ChangeBackgroundUrl(int sectionId, string url)
    {
        var section = await _sectionRepository.GetByAsyncId(sectionId);
        section!.ImageUrl = url;
        
        await _sectionRepository.UpdateAsync(section);
        return section.ImageUrl;
    }

    public async Task<SectionDto?> GetSectionWithTasksAsync(int sectionId)
    {
        var section = await _sectionRepository.GetSectionWithTasksAsync(sectionId);
        return _mapper.Map<SectionDto>(section);
    }
    
    public async Task<List<SectionDto>> GetSectionsByProjectAsync(int projectId)
    {
        var section = await _sectionRepository.GetSectionsWithTaskGroupsAsync(projectId);
        return _mapper.Map<List<SectionDto>>(section);
    }
}