using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models.AuthenticateRequests;

public class LoginRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}