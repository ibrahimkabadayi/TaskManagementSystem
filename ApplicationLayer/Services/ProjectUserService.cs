using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DomainLayer.Interfaces;

namespace Application.Services;

public class ProjectUserService : IProjectUserService
{
    private readonly IProjectUserRepository _projectUserRepository;
    private readonly IMapper _mapper;

    public ProjectUserService(IProjectUserRepository projectUserRepository, IMapper mapper)
    {
        _projectUserRepository = projectUserRepository;
        _mapper = mapper;
    }
    
    public async Task<ProjectUserDto?> GetProjectUserByIdAsync(int id)
    {
        var user = await _projectUserRepository.GetByAsyncId(id);
        return _mapper.Map<ProjectUserDto>(user) ?? null;
    }

    public async Task<List<ProjectUserDto?>> GetAllProjectsAsync()
    {
        var allProjectUsers = await _projectUserRepository.GetAllAsync();
        return _mapper.Map<List<ProjectUserDto>>(allProjectUsers)!;
    }
}