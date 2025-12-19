namespace DataAccessLayer.Entities;

public class Section
{
    public required int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<Task> Tasks { get; set; }
    public required int ProjectId { get; set; }
    public required Project Project { get; set; }
}