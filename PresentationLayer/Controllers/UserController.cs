using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public IActionResult CreateAccount([FromBody] BackToCreateAccountPageRequest request)
    {
        ViewBag.Username = request.Username;
        ViewBag.Email = request.Email;
        
        return View();
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

    [HttpPost]
    public IActionResult EmailCodeVerification(EmailRequest request)
    {
        ViewData["Email"] = request.Email;
        ViewData["Name"] = request.Name;
        
        return View();
    }

    [HttpPost]
    public IActionResult PasswordCreation([FromBody] AccountCheckRequest checkRequest)
    {
        ViewData["Name"] = checkRequest.Username;
        ViewData["Email"] = checkRequest.Email;
        
        return View();
    }
}