namespace DomainLayer.Entities;

public class Project
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
    public ICollection<Section> Sections { get; set; } = new List<Section>();
}