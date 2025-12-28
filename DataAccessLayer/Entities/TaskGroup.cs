namespace DataAccessLayer.Entities;

public class TaskGroup
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    
    public List<Task>? Tasks { get; set; }
    
    public int CreatedById { get; set; }
    public required ProjectUser CreatedBy { get; set; }
    
    public int SectionId { get; set; }
    public required Section Section { get; set; }
}