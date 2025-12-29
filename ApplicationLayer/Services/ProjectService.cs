using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DomainLayer.Interfaces;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    public ProjectService(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetByAsyncId(id);
        return project == null ? null : _mapper.Map<ProjectDto>(project);
    }

    public async Task<List<ProjectDto?>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        return (projects == null ? null : _mapper.Map<List<ProjectDto>>(projects))!;
    }
}