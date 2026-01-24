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

    [HttpGet("Task/GetTaskDetails/{taskId:int}")]
    public async Task<IActionResult> GetTaskDetails([FromRoute] int taskId)
    {
        var details = await _taskService.GetTaskDetailsAsync(taskId);
        return Json(details);
    }

    [HttpPost("Task/UpdatePriority")]
    public async Task<IActionResult> ChangeTaskPriority([FromBody] ChangeTaskPriorityRequest request)
    {
        var result = await _taskService.ChangeTaskPriority(request.TaskId, request.Priority);
        return (result == request.TaskId) ? Ok()  : BadRequest();
    }
    
    [HttpPost("Task/UpdateTaskState")]
    public async Task<IActionResult> ChangeTaskState([FromBody] ChangeTaskStateRequest request)
    {
        var state = request.State switch
        {
            "todo" => 0,
            "inprogress" => 1,
            "done" => 2,
            _ => -1
        };
        
        var result = await _taskService.ChangeTaskState(request.TaskId, state, request.UserId, request.ProjectId);
        return (result == request.TaskId) ? Ok()  : BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTask([FromBody] DeleteTaskRequest request)
    {
        var result = await _taskService.DeleteTask(request.TaskId, request.UserId, request.ProjectId);
        return (result == "Task deleted") ? Ok()  : BadRequest();
    }

    [HttpPost("Task/UpdateDescription")]
    public async Task<IActionResult> ChangeTaskDescription([FromBody] ChangeTaskDescriptionRequest request)
    {
        var result = await _taskService.ChangeTaskDescription(request.UserId, request.TaskId, request.ProjectId, request.Description);
        return (result == request.TaskId) ? Ok()  : BadRequest();
    }
    
    [HttpPost("Task/UpdateDueDate")]
    public async Task<IActionResult> ChangeTaskDueDate([FromBody] ChangeTaskDueDateRequest request)
    {
        var result = await _taskService.ChangeTaskDueDate(request.UserId, request.TaskId, request.ProjectId, request.DueDate);
        return (result == request.TaskId) ? Ok()  : BadRequest();
    }
    
    [HttpPost("Task/AssignUser")]
    public async Task<IActionResult> AssignUserToTask([FromBody] AssignUserToTaskRequest request)
    {
        var result = await _taskService.AssignUserToTask(request.UserId, request.TaskId, request.ProjectId, request.UserEmail);
        return (result == request.TaskId) ? Ok()  : BadRequest();
    }
    
    [HttpPost] 
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> SaveTask([FromBody] AddTaskRequest request)
    {
        var result = await _taskService.SaveTaskAsync(request.UserId, request.TaskTitle, request.TaskGroupName, request.SectionId);
        return (result.Title == request.TaskTitle) ? Ok(new { id = result.Id }) : BadRequest();
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeTaskGroup([FromBody] ChangeTaskGroupRequest request)
    {
        var result = await _taskService.ChangeTaskGroup(request.TaskId, request.TaskGroupId, request.NewPosition);
        return (result == request.TaskGroupId) ? Ok() : BadRequest();
    }
    
    [HttpPost("Task/UpdateTitle")]
    public async Task<IActionResult> UpdateTitle([FromBody] UpdateTaskTitleRequest request)
    {
        var result = await _taskService.UpdateTitle(request.TaskId, request.Title, request.UserId, request.ProjectId);
        return Ok(new { message = "Title updated successfully" });
    }
}