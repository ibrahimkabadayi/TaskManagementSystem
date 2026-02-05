using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.ProjectRequests;

namespace TaskManagementSystem.Controllers;

public class ProjectController : Controller
{
    private readonly IProjectService _projectService;
    
    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }
    
    [HttpPost]
    public async Task<IActionResult> InviteUser([FromBody] InviteUserRequest request)
    {
        var senderIdString = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(senderIdString)) return Unauthorized();
        
        var senderId = int.Parse(senderIdString);

        await _projectService.InviteUserToProjectAsync(request.ProjectId, senderId, request.EmailOrUsername, request.Role);
        
        return Ok(new { message = "Invitation sent successfully." });
    }

    [HttpPost("~/Project/RespondInvitation")]
    public async Task<IActionResult> RespondToInvitation(int invitationId, bool isAccepted)
    {
        try
        {
            await _projectService.RespondInvitationAsync(invitationId, isAccepted);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return BadRequest();
        }
        
        return Ok();
    }
    
    [HttpPost("~/Project/GenerateLink")]
    public async Task<IActionResult> GenerateLink([FromBody] int projectId)
    {
        var token = await _projectService.GenerateInviteLinkAsync(projectId);
    
        var joinUrl = $"{Request.Scheme}://{Request.Host}/join/{token}";
    
        return Ok(new { url = joinUrl });
    }

    [HttpPost("RevokeLink")]
    public async Task<IActionResult> RevokeLink([FromBody] int projectId)
    {
        await _projectService.RevokeInviteLinkAsync(projectId);
        return Ok(new { message = "Link başarıyla iptal edildi." });
    }

    [HttpGet("~/join/{token}")] 
    public async Task<IActionResult> JoinProjectByToken(string token)
    {
        Console.WriteLine(token);
        
        if (User.Identity is null || !User.Identity.IsAuthenticated)
        {
            return RedirectToAction("SignIn", "Home", new { returnUrl = $"/join/{token}" });
        }
        
        
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
        }

        var userIdString = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine($"ÇEKİLEN ID: '{userIdString}'");
        
        if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("SignIn", "Home");

        var userId = int.Parse(userIdString);

        var result = await _projectService.JoinProjectByTokenAsync(token, userId);

        return result ? RedirectToAction("TaskFlow", "Section", new { userId = userId }) : RedirectToAction("Home", "Home");
    }
}