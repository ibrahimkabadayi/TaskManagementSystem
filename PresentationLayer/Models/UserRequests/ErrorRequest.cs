namespace TaskManagementSystem.Models.UserRequests;

public class ErrorRequest
{
    public string? Message { get; set; }
    public string? Type { get; set; }
    public string? StatusCode { get; set; }
    public string? Timestamp { get; set; }
}