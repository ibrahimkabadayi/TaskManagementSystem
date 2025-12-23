namespace Application.DTOs;

public class TaskGroupDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<int> TaskIds { get; set; }
    public int CreatedById { get; set; }
    public int SectionId { get; set; }
}