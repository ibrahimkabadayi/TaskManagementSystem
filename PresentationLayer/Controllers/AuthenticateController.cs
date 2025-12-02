using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementSystem.Controllers;

[Route("Authenticate")]
public class AuthenticateController : Controller
{
    private readonly IUserService _userService;

    public AuthenticateController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("RegisterSignIn")]
    public async Task<IActionResult> RegisterSignIn([FromBody] SignInRequest request)
    {
        try
        {
            if (!CheckInfo(request.Password, request.Email))
            {
                return Json(new
                {
                    success = false,
                    message = "Wrong password",
                    errorCode = 401
                });
            }
         
            var user = await _userService.AuthenticateUserAsync(request.Email, request.Password);

            if (user == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Wrong password",
                    errorCode = 401
                });
            }
            
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.Name),
                new (ClaimTypes.Email, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = request.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(request.RememberMe ? 30 : 1)
            };
            
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Json(new
            {
                success = true,
                message = "Login successful",
                data = new
                {
                    name = user.Name,
                    email = user.Email,
                    redirectUrl = "/Home"
                }
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                message = "An error occurred during login",
                error = ex.Message,
                errorCode = 500
            });
        }
    }
    
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        return Json(new
        {
            success = true,
            message = "Logout successful",
            redirectUrl = "/Home"
        });
    }
    
    private static bool CheckInfo(string password, string email)
    {
        if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(email)
            && email.Length >= 3 && password.Length >= 3 && email.Contains("@gmail.com")) return true;
        Console.WriteLine("Invalid name or surname");
        return false;
    }
}