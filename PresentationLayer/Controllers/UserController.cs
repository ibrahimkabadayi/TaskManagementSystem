using Application.Services;
using AutoMapper;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers;

public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public IActionResult Error(ErrorRequest request)
    {
        ViewBag.Message = request.Message;
        ViewBag.Type = request.Type;
        ViewBag.StatusCode = request.StatusCode;
        ViewBag.Timestamp = request.Timestamp;
        
        return View();
    }
    
    public async Task<IActionResult> CreateAccountCheck(CreateAccountRequest request)
    {
        var userService = new UserService(_userRepository, _mapper);

        var isUserExists = await userService.CheckUserExistsAsync(request.Email);

        return isUserExists ? Json(new {success = false, message = "User already exists", error = "User exists", errorCode = "0001"}) : Json(new {success = true}); 
    }

    public IActionResult PasswordCreation(CreateAccountRequest request)
    {
        ViewData["Name"] = request.Username;
        ViewData["Email"] = request.Email;
        
        return View();
    }
}