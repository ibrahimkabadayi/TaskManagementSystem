namespace Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public List<int>? ProjectUserIds { get; set; }
    
    public string? ProfileColor { get; set; }
    public string? ProfileLetters { get; set; }
}