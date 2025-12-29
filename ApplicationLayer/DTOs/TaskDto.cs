using DomainLayer.Enums;

namespace Application.DTOs;

public class TaskDto
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CompletedDate { get; set; } 
    public TaskState State { get; set; } = TaskState.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Low;
    public int Position { get; set; } = 0;
    public int ProjectId { get; set; }
    
    public int CreatedById { get; set; }
    public UserDto? CreatedBy { get; set; }
    public int? AssignedToId { get; set; }
    public UserDto? AssignedTo { get; set; }
    public int? FinishedById { get; set; }
}