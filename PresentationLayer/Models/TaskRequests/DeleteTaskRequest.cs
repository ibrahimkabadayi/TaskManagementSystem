namespace TaskManagementSystem.Models.TaskRequests;

public class DeleteTaskRequest
{
    public required int TaskId { get; set; }
    public required int UserId { get; set; }
    public required int ProjectId { get; set; }
}