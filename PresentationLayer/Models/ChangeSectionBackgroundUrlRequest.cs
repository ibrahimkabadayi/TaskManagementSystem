namespace TaskManagementSystem.Models;

public class ChangeSectionBackgroundUrlRequest
{
    public int SectionId { get; set; }
    public required string Url { get; set; }
}