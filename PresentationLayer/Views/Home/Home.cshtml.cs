using System.Security.Claims;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskManagementSystem.Views;

public class Home : PageModel
{
    public UserDto UserData { get; set; } = new UserDto 
    { 
        Name = "default", 
        Email = "default" 
    };
    public void OnGet()
    {
        if (User.Identity?.IsAuthenticated != true) return;
        UserData.Name = User.FindFirst(ClaimTypes.Name)?.Value ?? "default";
        UserData.Email = User.FindFirst(ClaimTypes.Email)?.Value ?? "default";
    }
}