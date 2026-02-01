using DomainLayer.Enums;

namespace Application.DTOs;

public class ProjectInvitationDto
{
    public int Id { get; set; }
    
    public required DateTime CreatedDate { get; set; }
    
    public required ProjectDto Project { get; set; }
    
    public required UserDto Sender { get; set; }
    
    public required UserDto InvitedUser { get; set; }
    
    public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
}