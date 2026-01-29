using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models.AuthenticateRequests;

public class RegisterRequest
{
    public required string Name { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
}