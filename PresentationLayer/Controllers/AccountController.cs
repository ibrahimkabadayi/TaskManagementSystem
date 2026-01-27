using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.AccountRequests;

namespace TaskManagementSystem.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authService;
    
    public AccountController(IUserService userService, IAuthenticationService authService)
    {
        _userService = userService;
        _authService = authService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return RedirectToAction("SignIn", "Home"); 
        }
        
        var userDto = await _userService.GetUserWithProjectUsersAsync(1);

        if (userDto == null) return NotFound();

        return View(userDto);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile([FromBody] ChangeUserProfileRequest model)
    {
        var result = await _userService.UpdateUserProfileAsync(model.UserId, model.NewUserName, model.NewProfileColor!);
        
        if (!result)
        {
            return BadRequest(new { message = "Data could not be updated." });
        }
        
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentEmail = User.FindFirstValue(ClaimTypes.Email);
        var currentRole = User.FindFirstValue(ClaimTypes.Role);

        await _authService.RefreshUserSessionAsync(
            userId: currentUserId!,
            email: currentEmail!,
            newName: model.NewUserName,
            role: currentRole ?? "Member",
            newColor: model.NewProfileColor
        );
        
        TempData["Success"] = "Your profile was updated successfully!";
        
        return Ok(new { success = true });
    }
}