namespace DomainLayer.Entities;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; } 
    public string ProfileColor { get; set; } = "#2596be";
    
    public ICollection<ProjectUser>? ProjectUsers { get; set; } = new List<ProjectUser>();
}