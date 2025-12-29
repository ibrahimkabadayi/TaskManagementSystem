using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.TaskRequests;

namespace TaskManagementSystem.Controllers;

public class TaskController : Controller
{
    
    private readonly ITaskService _taskService;
    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTaskDetails(GetTaskDetailsRequest request)
    {
        var details = await _taskService.GetTaskDetailsAsync(request.TaskTitle, request.TaskGroupName, request.SectionName);
        return Json(details);
    }

    [HttpPost]
    public async Task<IActionResult> SaveTask(AddTaskRequest request)
    {
        var result = await _taskService.SaveTaskAsync(request.UserId, request.TaskTitle, request.TaskGroupName, request.SectionName);
        return (result.Title == request.TaskTitle)  ? Ok() : BadRequest();
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeTaskGroup(ChangeTaskGroupRequest request)
    {
        var result = await _taskService.ChangeTaskGroup(request.TaskId, request.TaskGroupId, request.NewPosition);
        return (result == request.TaskGroupId)? Ok() : BadRequest();
    }
}