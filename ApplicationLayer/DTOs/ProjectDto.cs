namespace Application.DTOs;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<int> ProjectUserIds { get; set; }
    public List<int> TaskIds { get; set; }
    public List<SectionDto> Sections { get; set; }
}