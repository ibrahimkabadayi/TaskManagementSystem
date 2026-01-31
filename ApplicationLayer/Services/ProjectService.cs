using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Enums;
using DomainLayer.Interfaces;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IUserRepository _userRepository;
    private readonly IProjectUserRepository _projectUserRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    public ProjectService(IProjectRepository projectRepository, IMapper mapper, IUserRepository userRepository, IProjectUserRepository projectUserRepository)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _projectUserRepository = projectUserRepository;
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

    public async Task<ProjectDto?> GetProjectWithSectionAsync(int id)
    {
        var project = await _projectRepository.GetProjectWithSectionAsync(id);
        return project == null ? null : _mapper.Map<ProjectDto>(project);
    }

    public async Task<List<ProjectDto>> GetAllProjectsOfUserAsync(int userId)
    {
        var projects = await _projectRepository.GetAllProjectsOfOneUserAsync(userId);
        return _mapper.Map<List<ProjectDto>>(projects);
    }

    public async Task<ProjectDto> CreateProjectAsync(string projectName, string description, int userId)
    {
        var newProject = new Project
        {
            Name = projectName,
            Description = description,
            StartDate = DateTime.Now
        };
        
        await _projectRepository.AddAsync(newProject);

        var newProjectUser = new ProjectUser
        {
            IsActive = true,
            JoinedDate = DateTime.Now,
            Role = ProjectRole.Leader,
            ProjectId = newProject.Id,
            UserId = userId,
        };

        await _projectUserRepository.AddAsync(newProjectUser);
        
        return _mapper.Map<ProjectDto>(newProject);
    }

    public async Task<int> UpdateProjectAsync(int projectId, string projectName, string description, string startDate, string endDate)
    {
        var project = await _projectRepository.GetByAsyncId(projectId);
        if (project == null) return -1;
        
        project.Name = projectName;
        project.Description = description;
        
        if (!string.IsNullOrWhiteSpace(startDate)) project.StartDate = DateTime.Parse(startDate);
        if (!string.IsNullOrWhiteSpace(endDate)) project.EndDate = DateTime.Parse(endDate);
        
        await _projectRepository.UpdateAsync(project);
        
        return project.Id;
    }

    public async Task<bool> DeleteProjectAsync(int projectId)
    {
        var project = await _projectRepository.GetByAsyncId(projectId);
        if (project is null) return false;

        try
        {
            var projectUsers = await _projectUserRepository.GetProjectUsersWithDetailsAsync(projectId);
            foreach (var projectUser in projectUsers)
                await _projectUserRepository.DeleteAsync(projectUser.Id);
            await _projectRepository.DeleteAsync(projectId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        
        return true;
    }
}