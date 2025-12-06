using DataAccessLayer.Enums;

namespace Application.DTOs;

public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CompletedDate { get; set; }
    public TaskState  State { get; set; }
    public TaskPriority Priority { get; set; }
    public int ProjectId { get; set; }
    public int CreatedById { get; set; }
    public int AssignedToId { get; set; }
    public int FinishedById { get; set; }
}