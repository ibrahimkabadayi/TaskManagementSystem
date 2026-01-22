namespace Application.DTOs;

public class TaskGroupDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<TaskDto>? TaskDtos { get; set; }
    public required int CreatedById { get; set; }
    public SectionDto? Section { get; set; }
}