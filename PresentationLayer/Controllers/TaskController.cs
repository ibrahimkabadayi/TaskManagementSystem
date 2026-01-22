using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.TaskRequests;

namespace TaskManagementSystem.Controllers;

public class TaskController : Controller
{
    private readonly ITaskService _taskService;
    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("GetTaskDetails/$TaskId")]
    public async Task<IActionResult> GetTaskDetails([FromQuery] GetTaskDetailsRequest request)
    {
        var details = await _taskService.GetTaskDetailsAsync(request.TaskId);
        return Json(details);
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeTaskPriority([FromBody] ChangeTaskPriorityRequest request)
    {
        var result = await _taskService.ChangeTaskPriority(request.TaskId, request.Priority);
        return (result == request.TaskId) ? Ok()  : BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTask([FromBody] DeleteTaskRequest request)
    {
        var result = await _taskService.DeleteTask(request.TaskId, request.UserId, request.ProjectId);
        return (result == "TaskDeleted") ? Ok()  : BadRequest();
    }
    
    [HttpPost] 
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> SaveTask([FromBody] AddTaskRequest request)
    {
        var result = await _taskService.SaveTaskAsync(request.UserId, request.TaskTitle, request.TaskGroupName, request.SectionId);
        return (result.Title == request.TaskTitle)  ? Ok() : BadRequest();
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeTaskGroup([FromBody] ChangeTaskGroupRequest request)
    {
        var result = await _taskService.ChangeTaskGroup(request.TaskId, request.TaskGroupId, request.NewPosition);
        return (result == request.TaskGroupId)? Ok() : BadRequest();
    }
}