namespace TaskManagementSystem.Models.ProjectRequests;

public class InviteUserRequest
{
    public int ProjectId { get; set; }
    public required string EmailOrUsername { get; set; }
    public required string Role { get; set; }
}