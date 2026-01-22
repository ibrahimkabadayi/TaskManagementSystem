using DomainLayer.Enums;

namespace DomainLayer.Entities;
public class Task
{
    public int Id { get; set; }
    
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }

    public TaskState State { get; set; } = TaskState.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Low;
    public int Position { get; set; }
    public required int TaskGroupId { get; set; }
    public virtual TaskGroup TaskGroup { get; set; }
    public required int CreatedById { get; set; }
    public virtual ProjectUser CreatedBy { get; set; }
    public int? AssignedToId { get; set; }
    public ProjectUser? AssignedTo { get; set; }
    public int? FinishedById { get; set; }
    public ProjectUser? FinishedBy { get; set; }
    
}