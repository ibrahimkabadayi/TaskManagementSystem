namespace TaskManagementSystem.Models.SectionRequests;

public class CreateSectionRequest
{
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int ProjectId { get; set; }
}