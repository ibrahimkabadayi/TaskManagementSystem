using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using TaskManagementSystem.Views;

namespace TaskManagementSystem.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Error(ErrorRequest request)
    {
        ViewBag.Message = request.Message;
        ViewBag.Type = request.Type;
        ViewBag.StatusCode = request.StatusCode;
        ViewBag.Timestamp = request.Timestamp;
        
        return View();
    }
    public IActionResult Home(UserDto user)
    {
        ViewBag.User = user;
        return View();
    }

    public async Task<IActionResult> RegisterSignIn(SignInRequest request)
    {
        if (!CheckInfo(request.Name, request.Password, request.Email))
        {
            return Json(new ApiResponse<UserDto>{Data = null, Success = false, Message = "Could not found User or wrong input"});
        }
        
        var userService = new UserService();
        var userDto = await userService.GiveAccessToUser(request.Name, request.Email, request.Password);
        
        return Json(new ApiResponse<UserDto>{Data = userDto, Success = true, Message = "Successfully gave access to user"});
    }
    
    private static bool CheckInfo(string name, string password, string email)
    {
        if (name.Length < 3 || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) ||
            string.IsNullOrEmpty(email)
            || email.Length < 3 || password.Length < 3 || !email.Contains("@gmail.com"))
        {
            Console.WriteLine("Invalid name or surname");
            return false;
        }
        else
        {
            return true;
        }
    }

}