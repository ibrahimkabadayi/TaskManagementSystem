namespace TaskManagementSystem.Models.TaskRequests;

public class ChangeTaskGroupRequest
{
    public required int TaskId { get; set; }
    public required int TaskGroupId { get; set; }
    public required int NewPosition { get; set; }
}