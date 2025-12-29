namespace TaskManagementSystem.Models.TaskRequests;

public class GetTaskDetailsRequest
{
    public required string TaskTitle { get; set; }
    public required string TaskGroupName { get; set; }
    public required string SectionName { get; set; }
}