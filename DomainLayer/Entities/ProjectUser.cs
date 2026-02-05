using DomainLayer.Enums;

namespace DomainLayer.Entities;

public class ProjectUser
{
    public int Id { get; set; }
    public required int ProjectId { get; set; }
    public Project Project { get; set; }
    public required int UserId { get; set; }
    public virtual User User { get; set; }
    public required ProjectRole Role { get; set; }
    public string Title  { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int AssignedTaskCount { get; set; }
    public int CompletedTaskCount { get; set; }
    public int PendingTaskCount { get; set; }
    public required DateTime JoinedDate { get; set; }
}