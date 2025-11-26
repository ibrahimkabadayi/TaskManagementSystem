using Application.DTOs;
using Application.Services;
using AutoMapper;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers;

public class AuthenticateController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public AuthenticateController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<IActionResult> RegisterSignIn(SignInRequest request)
    {
        if (!CheckInfo(request.Name, request.Password, request.Email))
        {
            return Json(new ApiResponse<UserDto>{Data = null, Success = false, Message = "Wrong input"});
        }
        
        var userService = new UserService(_userRepository, _mapper);
        try
        {
            var userDto = await userService.AuthenticateUserAsync(request.Name, request.Email, request.Password);
            return Json(new ApiResponse<UserDto>{Data = userDto, Success = true, Message = "Successfully gave access to user"});
        }
        catch (Exception ex)
        {
            return Json(new ApiResponse<UserDto>{Data = null, Success = false, Message = ex.Message});
        }
    }
    
    private static bool CheckInfo(string name, string password, string email)
    {
        if (name.Length >= 3 && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(password) &&
            !string.IsNullOrEmpty(email)
            && email.Length >= 3 && password.Length >= 3 && email.Contains("@gmail.com")) return true;
        Console.WriteLine("Invalid name or surname");
        return false;
    }
}