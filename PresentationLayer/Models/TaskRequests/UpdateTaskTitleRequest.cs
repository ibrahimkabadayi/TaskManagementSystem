namespace TaskManagementSystem.Models.TaskRequests;

public class UpdateTaskTitleRequest
{
    public int TaskId { get; set; }
    public required string Title { get; set; }
    public int UserId { get; set; }
    public int ProjectId { get; set; }
}