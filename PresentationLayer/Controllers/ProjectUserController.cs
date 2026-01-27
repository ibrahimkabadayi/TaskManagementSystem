using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.ProjectUserRequests;

namespace TaskManagementSystem.Controllers;

public class ProjectUserController : Controller
{
    private readonly IProjectUserService _projectUserService;

    public ProjectUserController(IProjectUserService projectUserService)
    {
        _projectUserService = projectUserService;
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequest request)
    {
        var result = await _projectUserService.ChangeRole(request.ProjectUserId, request.NewRole, request.UserId, request.ProjectId);
        return (result == request.ProjectUserId) ? Ok() : BadRequest("Error Id: " + result);
    }

    [HttpDelete("/RemoveMember/{idString}")]
    public async Task<IActionResult> RemoveMember([FromRoute] string idString)
    {
        var ids = idString.Split(' ');
        var projectUserId = int.Parse(ids[0]);
        var userId = int.Parse(ids[1]);
        var projectId = int.Parse(ids[2]);
        
        var result = await _projectUserService.RemoveProjectUser(projectUserId, userId, projectId);
        return (result == projectUserId) ? Ok() : BadRequest("Error Id: " + result);
    }
}