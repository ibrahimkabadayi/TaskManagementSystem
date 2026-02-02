using Application.DTOs;

namespace Application.Interfaces;

public interface IProjectInvitationService
{
    Task<ProjectInvitationDto?> GetProjectInvitationByIdAsync(int id);
    Task<int> CreateProjectInvitationAsync(int projectId, int invitedUserId, int senderUserId);
}