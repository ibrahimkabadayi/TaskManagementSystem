namespace DomainLayer.Entities;

public class Section
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<TaskGroup>? TaskGroups { get; set; } = [];
    public required int ProjectId { get; set; }
    public virtual Project Project { get; set; }
}