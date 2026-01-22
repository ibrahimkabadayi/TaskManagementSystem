namespace Application.DTOs;

public class SectionDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int ProjectId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public required List<TaskGroupDto> TasksGroupDtos { get; set; }
}