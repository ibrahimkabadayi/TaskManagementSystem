using Application.DTOs.Enums;

namespace Application.DTOs;

public class ProjectUserDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int UserId { get; set; }
    public ProjectRole Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime JoinedDate { get; set; }
}