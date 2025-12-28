using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Enums;

namespace DataAccessLayer.Entities;

public class Task
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }
    [MaxLength(1000)]
    public string? Description { get; set; }
    [Required, DataType("datetime")]
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