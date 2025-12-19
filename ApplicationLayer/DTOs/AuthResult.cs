namespace Application.DTOs;

public class AuthResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public UserInfo User { get; set; }
    public string? RedirectUrl { get; set; }
}