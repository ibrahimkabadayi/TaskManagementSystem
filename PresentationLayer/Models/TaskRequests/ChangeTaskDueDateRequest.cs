namespace TaskManagementSystem.Models.TaskRequests;

public class ChangeTaskDueDateRequest
{
    public int TaskId { get; set; }
    public required string DueDate { get; set; }
    public required int UserId { get; set; }
    public required int ProjectId { get; set; }
}