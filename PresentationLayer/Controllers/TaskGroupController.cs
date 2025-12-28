using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers;

public class TaskGroupController : Controller
{
    private readonly ITaskGroupService _taskGroupService;
    
    public TaskGroupController(ITaskGroupService taskGroupService)
    {
        _taskGroupService = taskGroupService;
    }

    [HttpPost]
    public async Task<IActionResult> SaveNewTaskGroup(AddTaskGroupRequest request)
    {
        var result = await _taskGroupService.SaveTaskGroupAsync(request.Name, request.SectionId, request.UserId);
        return (result.SectionId == request.SectionId) ? Ok(result) : BadRequest(result);
    }
}