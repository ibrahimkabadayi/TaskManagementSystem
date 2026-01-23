namespace TaskManagementSystem.Models.TaskRequests;

public class ChangeTaskDescriptionRequest
{
    public required string Description { get; set; }
    public int TaskId { get; set; }
    public int UserId { get; set; }
    public int ProjectId { get; set; }
}