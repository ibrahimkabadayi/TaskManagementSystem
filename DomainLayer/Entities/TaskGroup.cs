namespace DomainLayer.Entities;

public class TaskGroup
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    
    public List<Task>? Tasks { get; set; }
    
    public required int CreatedById { get; set; }
    public virtual ProjectUser CreatedBy { get; set; }
    
    public required int SectionId { get; set; }
    public virtual Section Section { get; set; }
}