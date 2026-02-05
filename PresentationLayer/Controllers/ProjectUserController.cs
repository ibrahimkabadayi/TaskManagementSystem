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

    [HttpDelete]
    public async Task<IActionResult> RemoveMember(int projectUserId, int userId, int projectId)
    {
        
        var result = await _projectUserService.RemoveProjectUser(projectUserId, userId, projectId);
        return (result == projectUserId) ? Ok() : BadRequest("Error Id: " + result);
    }
}