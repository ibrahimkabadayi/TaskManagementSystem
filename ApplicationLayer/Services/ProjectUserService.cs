using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DomainLayer.Enums;
using DomainLayer.Interfaces;

namespace Application.Services;

public class ProjectUserService : IProjectUserService
{
    private readonly IProjectUserRepository _projectUserRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public ProjectUserService(IProjectUserRepository projectUserRepository, IUserRepository userRepository, IMapper mapper)
    {
        _projectUserRepository = projectUserRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<ProjectUserDto?> GetProjectUserByIdAsync(int id)
    {
        var user = await _projectUserRepository.GetByAsyncId(id);
        return _mapper.Map<ProjectUserDto>(user) ?? null;
    }

    public async Task<List<ProjectUserDto?>> GetAllProjectUsersOfOneProjectAsync(int projectId)
    {
        var allProjectUsers = await _projectUserRepository.GetProjectUsersWithDetailsAsync(projectId);
        return _mapper.Map<List<ProjectUserDto>>(allProjectUsers)!;
    }

    public async Task<List<ProjectUserDetailsDto>> GetAllProjectUserDetailsOfOneProjectAsync(int projectId)
    {
        var allProjectUsers = await GetAllProjectUsersOfOneProjectAsync(projectId);

        List<ProjectUserDetailsDto> details = [];
        details.AddRange(allProjectUsers.Select(projectUser => new ProjectUserDetailsDto
        {
            Id = projectUser!.Id,
            FullName = projectUser!.User.Name!,
            Email = projectUser!.User.Email!,
            ProfileColor = projectUser!.User.ProfileColor!,
            Role = projectUser.Role.ToString(),
            IsActive = projectUser.IsActive.ToString()
        }));

        return details;
    }

    public async Task<int> ChangeRole(int projectUserId, string newRole, int userId, int projectId)
    {
        var projectUser = await _projectUserRepository.FindFirstAsync(x => x.UserId == userId && x.ProjectId == projectId);

        if (projectUser!.Role != ProjectRole.Leader) return -1;
        
        projectUser.Role = Enum.Parse<ProjectRole>(newRole);
        await _projectUserRepository.UpdateAsync(projectUser);
        
        return projectUser.Id;
    }

    public async Task<int> RemoveProjectUser(int projectUserId, int userId, int projectId)
    {
        var projectUser = await _projectUserRepository.FindFirstAsync(x => x.UserId == userId && x.ProjectId == projectId);

        if (projectUser!.Role != ProjectRole.Leader && projectUser.Id != projectUserId) return -1;

        await _projectUserRepository.DeleteAsync(projectUserId);

        return projectUserId;
    }
}