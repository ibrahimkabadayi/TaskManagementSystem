namespace TaskManagementSystem.Models.EmailRequests;

public class VerifyCodeRequest
{
    public string Email { get; set; } = string.Empty;
    public string EnteredCode { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}