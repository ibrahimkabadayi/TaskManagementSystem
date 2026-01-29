namespace TaskManagementSystem.Models.UserRequests;

public class AccountCheckRequest
{
    public required string Username { get; set; }
    public required string Email { get; set; }
}