using DomainLayer.Enums;

namespace DomainLayer.Entities;

public class ProjectUser
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public required Project Project { get; set; }
    public int UserId { get; set; }
    public required User User { get; set; }
    public required ProjectRole Role { get; set; }
    public string Title  { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public required DateTime JoinedDate { get; set; }
}