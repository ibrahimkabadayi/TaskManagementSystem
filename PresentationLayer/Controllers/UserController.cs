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
    
    public IActionResult Error(ErrorRequest request)
    {
        ViewBag.Message = request.Message;
        ViewBag.Type = request.Type;
        ViewBag.StatusCode = request.StatusCode;
        ViewBag.Timestamp = request.Timestamp;
        
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(CreateAccountRequest request)
    {
        var newUser = new UserDto{Name = request.Name, Email = request.Email, Password = request.Password};
        var savedUser = await _userService.RegisterUserAsync(newUser);

        if (savedUser != null)
        {
            var apiResponse = new ApiResponse<UserDto>{Data = newUser, Success = true, Message = "User Added Successfully"};
            return Json(new {apiResponse});
        }
        else
        {
            var apiResponse = new ApiResponse<UserDto>{Data = newUser, Success = false, Message = "User Not Added"};
            return Json(new {apiResponse});
        }
    }
    public async Task<IActionResult> CreateAccountCheck(AccountCheckRequest checkRequest)
    {
        var isUserExists = await _userService.CheckUserExistsAsync(checkRequest.Email);

        return isUserExists ? Json(new {success = false, message = "User already exists", error = "User exists", errorCode = "0001"}) : Json(new {success = true}); 
    }

    public IActionResult PasswordCreation(AccountCheckRequest checkRequest)
    {
        ViewData["Name"] = checkRequest.Username;
        ViewData["Email"] = checkRequest.Email;
        
        return View();
    }
}