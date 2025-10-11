using DataAccessLayer.Enums;

namespace DataAccessLayer.Entities;

public class Task
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    
    public TaskState State { get; set; }
    public TaskPriority Priority { get; set; }
    
    public required User CreatedBy { get; set; }
    public User? AssignedTo { get; set; }
    public User? FinishedBy { get; set; }
    public int CreatedById { get; set; }
    public int AssignedToId { get; set; }
    public int? FinishedById { get; set; }
}