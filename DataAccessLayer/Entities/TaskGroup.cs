namespace DataAccessLayer.Entities;

public class TaskGroup
{
    public required int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    
    public List<Task>? Tasks { get; set; }
    
    public int CreatedById { get; set; }
    public virtual User CreatedBy { get; set; }
    
    public int SectionId { get; set; }
    public virtual Section Section { get; set; }
}