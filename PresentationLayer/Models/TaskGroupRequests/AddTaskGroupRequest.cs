namespace TaskManagementSystem.Models.TaskGroupRequests;

public class AddTaskGroupRequest
{
    public required string Name { get; set; }
    public int SectionId { get; set; }
    
    public int UserId { get; set; }
}