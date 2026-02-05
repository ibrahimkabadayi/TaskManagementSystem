using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DomainLayer.Enums;
using DomainLayer.Interfaces;

namespace Application.Services;

public class ProjectUserService : IProjectUserService
{
    private readonly IProjectUserRepository _projectUserRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    public ProjectUserService(IProjectUserRepository projectUserRepository, IUserRepository userRepository, IMapper mapper, INotificationService notificationService, IProjectRepository projectRepository, ITaskRepository taskRepository)
    {
        _projectUserRepository = projectUserRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _notificationService = notificationService;
        _projectRepository = projectRepository;
        _taskRepository = taskRepository;
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
        
        var changeRoleUser = await _projectUserRepository.GetByAsyncId(projectUserId);
        
        if (changeRoleUser == null) return -1;
        
        changeRoleUser.Role = Enum.Parse<ProjectRole>(newRole);
        await _projectUserRepository.UpdateAsync(changeRoleUser);
        
        return changeRoleUser.Id;
    }

    public async Task<int> RemoveProjectUser(int projectUserId, int userId, int projectId)
    {
        var projectUser = await _projectUserRepository.FindFirstAsync(x => x.UserId == userId && x.ProjectId == projectId);
        
        var project = await _projectRepository.GetByAsyncId(projectId);
        if (project == null) return -1;

        if (projectUser!.Role != ProjectRole.Leader && projectUser.Id != projectUserId) return -1;
        
        var user = await _userRepository.GetByAsyncId(projectUser.UserId);
        if (user == null) return -1;
        
        await _notificationService.CreateNotificationAsync(user.Id, "Removed from project", $"You have been removed from project {project.Name}", null, null, NotificationType.Info);
        
        var assignedTasks = await _taskRepository.FindAsync(x => x.AssignedToId == projectUserId);

        foreach (var task in assignedTasks)
            task.AssignedToId = null;
        

        await _projectUserRepository.DeleteAsync(projectUserId);

        return projectUserId;
    }
}