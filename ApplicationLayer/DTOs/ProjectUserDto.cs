using DomainLayer.Enums;

namespace Application.DTOs;

public class ProjectUserDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public required UserDto User { get; set; }
    public ProjectRole Role { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime JoinedDate { get; set; }
}