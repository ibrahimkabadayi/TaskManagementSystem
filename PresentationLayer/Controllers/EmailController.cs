using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.EmailRequests;

namespace TaskManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;
    
    public EmailController(IEmailService emailService,  IUserService userService)
    {
        _emailService = emailService;
        _userService = userService;
    }

    [HttpPost("send-verification-code")]
    public async Task<IActionResult> SendEmailVerificationCode([FromBody] SendCodeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var isExists = await _userService.CheckUserExistsAsync(request.Email);

        if (!isExists)
        {
            return BadRequest(new 
            { 
                success = false,
                message = "User does not exist" 
            });
        }

        var code = await _emailService.SendCode(request.Email);
        
        HttpContext.Session.SetString($"EmailCode_{request.Email}", code);
        HttpContext.Session.SetString($"EmailCodeExpiry_{request.Email}", 
            DateTime.UtcNow.AddMinutes(10).ToString());

        return Ok(new { success = true });
    }
    
    [HttpPost("VerifyCode")]
    public IActionResult VerifyCode([FromBody] VerifyCodeRequest request)
    {
        try
        {
            var savedCode = HttpContext.Session.GetString($"EmailCode_{request.Email}");
            var expiry = HttpContext.Session.GetString($"EmailCodeExpiry_{request.Email}");
        
            if (string.IsNullOrEmpty(savedCode))
            {
                return BadRequest(new { success = false, message = "Could not find email code." });
            }
            
            if (DateTime.Parse(expiry!) < DateTime.UtcNow)
            {
                return BadRequest(new { success = false, message = "Code time is up." });
            }

            if (savedCode != request.EnteredCode) return BadRequest(new { success = false, message = "Incorrect Code" });
            HttpContext.Session.Remove($"EmailCode_{request.Email}");
            HttpContext.Session.Remove($"EmailCodeExpiry_{request.Email}");
            
            return Ok(new { 
                success = true,
                message = "Email confirmed",
                email = request.Email, 
                name = request.UserName});
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("send-test-email")]
    public async Task<IActionResult> SendTestEmail([FromBody] TestEmailRequest request)
    {
        try
        {
            await _emailService.SendEmail(
                request.Email, 
                "Test Email", 
                "<h1>This is a test email!</h1>");

            return Ok(new { success = true, message = "Email sent" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
    [HttpPost("send-welcome")]
    public async Task<IActionResult> SendWelcomeEmail([FromBody] WelcomeEmailRequest request)
    {
        try
        {
            await _emailService.SendWelcomeEmail(request.Email, request.Name);
            return Ok(new { success = true, message = "Welcome Email has sent." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}