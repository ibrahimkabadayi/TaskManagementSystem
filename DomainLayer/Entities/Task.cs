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
    
    public TaskState State { get; set; }
    public TaskPriority Priority { get; set; }

    public int Position { get; set; } = 0;
    
    public int TaskGroupId { get; set; }
    public virtual required TaskGroup TaskGroup { get; set; }
    
    public int CreatedById { get; set; }
    public virtual required ProjectUser CreatedBy { get; set; }
    
    public int AssignedToId { get; set; }
    public virtual ProjectUser? AssignedTo { get; set; }
    
    public int? FinishedById { get; set; }
    public virtual ProjectUser? FinishedBy { get; set; }
    
}