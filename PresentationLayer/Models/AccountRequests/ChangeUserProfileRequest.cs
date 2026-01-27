namespace TaskManagementSystem.Models.AccountRequests;

public class ChangeUserProfileRequest
{
    public int UserId { get; set; }
    public string NewUserName { get; set; } = string.Empty;
    public string NewProfileColor { get; set; } = string.Empty;
}