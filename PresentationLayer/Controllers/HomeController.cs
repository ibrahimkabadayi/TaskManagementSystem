using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementSystem.Controllers;

public class HomeController : Controller
{
    private readonly IUserService _userService;

    public HomeController(IUserService userService)
    {
        _userService = userService;
    }
    public IActionResult SignIn()
    {
        return View();
    }

    public async Task<IActionResult> Home()
    {
        if (User.Identity?.IsAuthenticated != true) return View();
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
        ViewBag.UserId = userId;

        var user = await _userService.GetUserByIdAsync(int.Parse(userId!));
        
        if (user == null) return View();
        
        ViewBag.ProfileLetters = user.ProfileLetters;
        ViewBag.ProfileColor = user.ProfileColor;

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
    public IActionResult Privacy()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult EmailCodeVerification(string email, string name)
    {
        ViewData["Email"] = email;
        ViewData["Name"] = name;
        
        return View();
    }
    
    [HttpGet]
    public IActionResult PasswordCreation(string userName, string email)
    {
        ViewData["Name"] = userName;
        ViewData["Email"] = email;
        
        return View();
    }
    
    [HttpGet]
    public IActionResult CreateAccount(string username, string email)
    {
        ViewBag.Username = username;
        ViewBag.Email = email;
        
        return View();
    }
}