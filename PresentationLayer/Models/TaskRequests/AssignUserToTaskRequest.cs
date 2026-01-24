namespace TaskManagementSystem.Models.TaskRequests;

public class AssignUserToTaskRequest
{
    public required string UserEmail { get; set; }
    public int TaskId { get; set; }
    public int ProjectId { get; set; }
    public int UserId { get; set; }
}