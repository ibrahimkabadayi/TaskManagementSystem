using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.TaskGroupRequests;

namespace TaskManagementSystem.Controllers;

public class TaskGroupController : Controller
{
    private readonly ITaskGroupService _taskGroupService;
    
    public TaskGroupController(ITaskGroupService taskGroupService)
    {
        _taskGroupService = taskGroupService;
    }

    [HttpPost]
    public async Task<IActionResult> SaveNewTaskGroup([FromBody] AddTaskGroupRequest request)
    {
        var taskGroupDto = await _taskGroupService.SaveTaskGroupAsync(request.Name, request.SectionId, request.UserId);
        return Ok(new { id = taskGroupDto!.Id });
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeTaskGroupName([FromBody] ChangeTaskGroupNameRequest request)
    {
        var result = await _taskGroupService.ChangeTaskGroupNameAsync(request.TaskGroupId, request.NewTaskGroupName);
        return (result.Name == request.NewTaskGroupName) ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("TaskGroup/DeleteTaskGroup/{taskGroupId:int}")]
    public async Task<IActionResult> DeleteTaskGroup(int taskGroupId)
    {
        await _taskGroupService.DeleteTaskGroupAsync(taskGroupId);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> GetTaskStartDates([FromBody] GetTaskDatesRequest request)
    {
        var allTasks = await _taskGroupService.GetAllTasksAsync(request.TaskGroupId);

        var taskDates = allTasks.Select(task => new 
        { 
            task.Id, 
            task.StartDate 
        }).ToList();

        return Ok(taskDates);
    }

    [HttpPost]
    public async Task<IActionResult> GetTaskPriorities([FromBody] GetTaskPrioritiesRequest request)
    {
        var allTasks = await _taskGroupService.GetAllTasksAsync(request.TaskGroupId);
        
        var taskPriorities = allTasks.Select(task => new 
        { 
            task.Id, 
            task.Priority 
        }).ToList();
        
        return Ok(taskPriorities);
    }
}