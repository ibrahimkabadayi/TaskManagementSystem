namespace TaskManagementSystem.Models.SectionRequests;

public class CreateProjectRequest
{
    public string Description { get; set; } = string.Empty;
    public required string Name { get; set; }
}