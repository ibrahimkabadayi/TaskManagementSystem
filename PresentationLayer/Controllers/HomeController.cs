using System.Security.Claims;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementSystem.Controllers;

public class HomeController : Controller
{
    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult Home()
    {
        if (!User.Identity.IsAuthenticated) return View();
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
            
        ViewBag.UserId = userId;
        ViewBag.Username = username;
        ViewBag.Email = email;

        return View();
    }
    public IActionResult TaskFlow(UserDto user)
    {
        ViewData["User"] = user;
        return View();
    }
    public IActionResult CreateAccount()
    {
        return View();
    }
}