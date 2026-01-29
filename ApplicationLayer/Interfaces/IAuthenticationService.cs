using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IAuthenticationService
{
    Task<AuthResult> RegisterAndLoginAsync(string name, string email, string password, HttpContext context);
    Task<AuthResult> LoginAsync(string email, string password, HttpContext context);
    Task RefreshUserSessionAsync(string userId, string email, string newName, string role, string newColor);
    Task LogoutAsync();
}