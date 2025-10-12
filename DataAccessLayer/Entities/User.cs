using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public class User
{
    public int Id { get; set; }
    [MaxLength (50)]
    public required string Name { get; set; }
    [MaxLength (50)]
    public required string Email { get; set; }
    [MaxLength (50)]
    public required string Password { get; set; }
    
    public ICollection<ProjectUser>? ProjectUsers { get; set; } = new List<ProjectUser>();
}