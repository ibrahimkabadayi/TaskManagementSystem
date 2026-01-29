using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.AuthenticateRequests;
using IAuthenticationService = Application.Interfaces.IAuthenticationService;

namespace TaskManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticateController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    
    public AuthenticateController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("~/Authenticate/Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _authenticationService.RegisterAndLoginAsync(
            request.Name,
            request.Email,
            request.Password,
            HttpContext  
        );
        
        if (!result.Success)
        {
            return BadRequest(new 
            { 
                success = false,
                message = result.Message 
            });
        }
        
        return Ok(new
        {
            success = true,
            message = result.Message,
            redirectUrl = result.RedirectUrl,
            user = result.User
        });
    }

    [HttpPost("~/Authenticate/Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _authenticationService.LoginAsync(request.Email, request.Password, HttpContext);
        
        if (!result.Success)
        {
            return Unauthorized(new 
            { 
                success = false,
                message = result.Message 
            });
        }
        
        return Ok(new
        {
            success = true,
            message = result.Message,
            redirectUrl = result.RedirectUrl,
            user = result.User
        });
    }
    
    [HttpGet("~/Authenticate/Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        return RedirectToAction("Home", "Home");
    }
}