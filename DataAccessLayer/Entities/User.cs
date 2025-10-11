namespace DataAccessLayer.Entities;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    
    public ICollection<ProjectUser>? ProjectUsers { get; set; } = new List<ProjectUser>();
}