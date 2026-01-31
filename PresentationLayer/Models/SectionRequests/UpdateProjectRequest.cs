namespace TaskManagementSystem.Models.SectionRequests;

public class UpdateProjectRequest
{
    public int ProjectId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string StartDate { get; set; }
    public string? EndDate { get; set; }
}