using DataAccessLayer.Enums;

namespace DataAccessLayer.Entities;

public class ProjectUser
{
    public int ProjectId { get; set; }
    public required Project Project { get; set; }
    
    public int UserId { get; set; }
    public required User User { get; set; }
    
    public required ProjectRole Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime JoinedDate { get; set; }
}