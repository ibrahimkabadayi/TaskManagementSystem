namespace TaskManagementSystem.Models.TaskRequests;

public class ChangeTaskStateRequest
{
    public int TaskId { get; set; }
    public required string State { get; set; }
    public required int UserId { get; set; }
    public required int ProjectId { get; set; }
}