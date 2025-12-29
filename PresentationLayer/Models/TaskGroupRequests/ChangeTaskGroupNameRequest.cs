namespace TaskManagementSystem.Models.TaskGroupRequests;

public class ChangeTaskGroupNameRequest
{
    public int TaskGroupId { get; set; }
    public required string NewTaskGroupName { get; set; }
}