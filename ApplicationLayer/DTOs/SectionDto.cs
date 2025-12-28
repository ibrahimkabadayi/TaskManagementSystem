using NPOI.SS.Formula.Functions;

namespace Application.DTOs;

public class SectionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public List<TaskGroupDto> TasksGroupDtos { get; set; } = null!;
}