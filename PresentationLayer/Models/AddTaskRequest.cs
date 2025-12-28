namespace TaskManagementSystem.Models;

public class AddTaskRequest
{
    public int UserId { get; set; }
    public required string TaskTitle { get; set; }
    public required string TaskGroupName { get; set; }
    public required string SectionName { get; set; }
}