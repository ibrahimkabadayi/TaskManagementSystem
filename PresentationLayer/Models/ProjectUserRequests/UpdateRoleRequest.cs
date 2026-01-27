namespace TaskManagementSystem.Models.ProjectUserRequests;

public class UpdateRoleRequest
{
    public int ProjectUserId { get; set; }
    public string NewRole { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int ProjectId { get; set; }
}