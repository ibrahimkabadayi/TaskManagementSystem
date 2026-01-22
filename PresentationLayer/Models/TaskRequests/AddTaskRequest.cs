namespace TaskManagementSystem.Models.TaskRequests;

public class AddTaskRequest
{
    public int UserId { get; set; }
    public required string TaskTitle { get; set; }
    public required string TaskGroupName { get; set; }
    public required int SectionId { get; set; }
}