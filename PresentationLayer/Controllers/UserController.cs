using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.UserRequests;

namespace TaskManagementSystem.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    
    
    [HttpPost]
    public IActionResult Error(ErrorRequest request)
    {
        ViewBag.Message = request.Message;
        ViewBag.Type = request.Type;
        ViewBag.StatusCode = request.StatusCode;
        ViewBag.Timestamp = request.Timestamp;
        
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAccountCheck([FromBody] AccountCheckRequest checkRequest)
    {
        var isUserExists = await _userService.CheckUserExistsAsync(checkRequest.Email);

        return isUserExists ? Json(new {success = false, message = "User already exists", error = "User exists", errorCode = "0001"}) : Json(new {success = true}); 
    }
}