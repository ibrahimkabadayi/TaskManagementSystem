namespace TaskManagementSystem.Models.TaskRequests;

public class ChangeTaskPriorityRequest
{
    public required int TaskId { get; set; }
    public required string Priority { get; set; }
}