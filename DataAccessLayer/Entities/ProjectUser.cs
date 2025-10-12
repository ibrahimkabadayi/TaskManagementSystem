using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Enums;

namespace DataAccessLayer.Entities;

public class ProjectUser
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    [Required]
    public required Project Project { get; set; }
    public int UserId { get; set; }
    [Required]
    public required User User { get; set; }
    [Required]
    public required ProjectRole Role { get; set; }
    [Required]
    public required bool IsActive { get; set; }
    [Required]
    public required DateTime JoinedDate { get; set; }
}