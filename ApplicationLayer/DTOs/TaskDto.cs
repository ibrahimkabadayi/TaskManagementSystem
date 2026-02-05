using DomainLayer.Enums;

namespace Application.DTOs;

public class TaskDto
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; } 
    public TaskState State { get; set; } = TaskState.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Low;
    public int Position { get; set; }
    public int TaskGroupId { get; set; }
    
    public UserDto? CreatedBy { get; set; }
    public ProjectUserDto? AssignedTo { get; set; }
    public ProjectUserDto? FinishedBy { get; set; }
}