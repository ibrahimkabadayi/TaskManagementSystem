using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Enums;
using DomainLayer.Interfaces;

namespace Application.Services;

public class ProjectInvitationService : IProjectInvitationService
{
    private readonly IProjectInvitationRepository _projectInvitationRepository;
    private readonly IMapper _mapper;
    
    public ProjectInvitationService(IProjectInvitationRepository projectInvitationRepository, IMapper mapper)
    {
        _projectInvitationRepository = projectInvitationRepository;
        _mapper = mapper;
    }
    
    public async Task<int> CreateProjectInvitationAsync(int projectId, int invitedUserId, int senderUserId, string role)
    {
        var projectRole = role == "Viewer" ? ProjectRole.Viewer : ProjectRole.Developer;
        
        var projectInvitation = new ProjectInvitation
        {
            CreatedDate = DateTime.Now,
            InvitedUserId = invitedUserId,
            SenderId = senderUserId,
            ProjectId = projectId,
            InvitedRole = projectRole
        };
        
        await _projectInvitationRepository.AddAsync(projectInvitation);

        return projectInvitation.Id;
    }
    
    public async Task<ProjectInvitationDto?> GetProjectInvitationByIdAsync(int id)
    {
        var invitation = await _projectInvitationRepository.GetByAsyncId(id);
        return _mapper.Map<ProjectInvitationDto>(invitation) ?? null;
    }
}