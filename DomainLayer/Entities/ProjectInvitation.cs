using DomainLayer.Enums;

namespace DomainLayer.Entities;

public class ProjectInvitation
{
    public int Id { get; set; }
    public required DateTime CreatedDate { get; set; }
    
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    
    public int SenderId { get; set; }
    public User Sender { get; set; }
    
    public int InvitedUserId { get; set; }
    public User InvitedUser { get; set; }
    
    public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
}