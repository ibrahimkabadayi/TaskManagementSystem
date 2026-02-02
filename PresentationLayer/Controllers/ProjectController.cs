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

        await _projectService.InviteUserToProjectAsync(request.ProjectId, senderId, request.EmailOrUsername);
        
        return Ok(new { message = "Invitation sent successfully." });
    }

    [HttpPost]
    public async Task<IActionResult> RespondToInvitation([FromBody] ResponseRequest request)
    {
        try
        {
            await _projectService.RespondInvitationAsync(request.InvitationId, request.IsAccepted);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return BadRequest();
        }
        
        return Ok();
    }
}