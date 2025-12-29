using DomainLayer.Enums;

namespace DomainLayer.Entities;

public class ProjectUser
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public virtual required Project Project { get; set; }
    public int UserId { get; set; }
    public virtual required User User { get; set; }
    public required ProjectRole Role { get; set; }
    public required bool IsActive { get; set; }
    public required DateTime JoinedDate { get; set; }
}