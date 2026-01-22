namespace Application.DTOs;

public class ProjectUserDetailsDto
{
    public required int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public required string ProfileColor { get; set; }
}