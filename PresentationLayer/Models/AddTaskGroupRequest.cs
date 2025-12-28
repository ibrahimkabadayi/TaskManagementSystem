namespace TaskManagementSystem.Models;

public class AddTaskGroupRequest
{
    public string Name { get; set; }
    public int SectionId { get; set; }
    
    public int UserId { get; set; }
}