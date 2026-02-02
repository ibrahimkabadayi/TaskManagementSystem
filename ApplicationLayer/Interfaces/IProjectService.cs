using Application.DTOs;

namespace Application.Interfaces;

public interface IProjectService
{
    Task<ProjectDto?> GetProjectByIdAsync(int id);
    Task<List<ProjectDto?>> GetAllProjectsAsync();
    Task<ProjectDto?> GetProjectWithSectionAsync(int id);
    Task<List<ProjectDto>> GetAllProjectsOfUserAsync(int userId);
    Task<ProjectDto> CreateProjectAsync(string projectName, string description, int userId);
    Task<int> UpdateProjectAsync(int projectId, string projectName, string description, string startDate, string endDate);
    Task<bool> DeleteProjectAsync(int projectId);
    Task InviteUserToProjectAsync(int projectId, int senderUserId, string emailOrUserName);
    Task RespondInvitationAsync(int invitationId, bool isAccepted);
}