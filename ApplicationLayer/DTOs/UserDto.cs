namespace Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public List<ProjectUserDto>? ProjectUsers { get; set; }
    
    public string? ProfileColor { get; set; }
    public string? ProfileLetters { get; set; }
}