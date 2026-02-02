using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Enums;
using DomainLayer.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IUserRepository _userRepository;
    private readonly IProjectUserRepository _projectUserRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectInvitationService _projectInvitationService;
    private readonly INotificationService _notificationService;
    private readonly IProjectInvitationRepository _projectInvitationRepository;
    private readonly IMapper _mapper;
    public ProjectService(IProjectRepository projectRepository, IMapper mapper, IUserRepository userRepository, IProjectUserRepository projectUserRepository, IProjectInvitationService projectInvitationService, INotificationService notificationService, IProjectInvitationRepository projectInvitationRepository)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _projectUserRepository = projectUserRepository;
        _projectInvitationService = projectInvitationService;
        _notificationService = notificationService;
        _projectInvitationRepository = projectInvitationRepository;
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

    public async Task InviteUserToProjectAsync(int projectId, int senderUserId, string emailOrUserName)
    {
        var project = await _projectRepository.GetByAsyncId(projectId);
        if (project is null) return;
        
        var senderUser = await _userRepository.GetByAsyncId(senderUserId);
        if (senderUser is null) return;

        User? invitedUser;
        
        if (emailOrUserName.Contains("@gmail.com"))
        {
            invitedUser = await _userRepository.GetByEmailAsync(emailOrUserName);
        }
        else
        {
            invitedUser = await _userRepository.FindFirstAsync(x => x.Name == emailOrUserName);
        }
        
        if (invitedUser is null) return;

        var projectUsers = await _projectUserRepository.GetProjectUsersWithDetailsAsync(projectId);
        
        if (projectUsers.Any(projectUser => projectUser.UserId == invitedUser.Id))
        {
            return;
        }
        
        var invitationId = await _projectInvitationService.CreateProjectInvitationAsync(projectId, invitedUser.Id, senderUserId);
        
        await _notificationService.CreateNotificationAsync(invitedUser.Id, "Project Invitation 📩",
            $"You have been invited to project {project.Name} by user {senderUser.Name}", null, invitationId
            ,NotificationType.Invitation);
    }

    public async Task RespondInvitationAsync(int invitationId, bool isAccepted)
    {
        var invitation = await _projectInvitationRepository.GetByAsyncId(invitationId);
        if (invitation is null) throw new Exception("Could not find invitation.");

        if (invitation.Status != InvitationStatus.Pending)
        {
            throw new Exception("Invitation is already answered or declined.");
        }

        if (isAccepted)
        {
            invitation.Status = InvitationStatus.Accepted;
            var projectUser = new ProjectUser
            {
                IsActive = true,
                JoinedDate = DateTime.Now,
                Role = ProjectRole.Developer,
                ProjectId = invitation.ProjectId,
                UserId = invitation.InvitedUserId
            };
            
            await _projectUserRepository.AddAsync(projectUser);
        }
        else
        {
            invitation.Status = InvitationStatus.Declined;
        }
        
        await _projectInvitationRepository.UpdateAsync(invitation);
    }
}