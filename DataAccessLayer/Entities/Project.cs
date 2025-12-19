using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public class Project
{
    public int Id { get; set; }
    [MaxLength(100)]
    public required string Name { get; set; }
    [MaxLength(1000)]
    public string? Description { get; set; }
    [Required]
    public required DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
    public ICollection<Section> Sections { get; set; } = new List<Section>();
}