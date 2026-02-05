namespace Application.DTOs;

public class ProjectDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? InviteToken { get; set; }
    public required DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<ProjectUserDto> ProjectUsers { get; set; } = [];
    public List<SectionDto> Sections { get; set; } = [];
}