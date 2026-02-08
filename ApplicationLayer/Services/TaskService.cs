using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DomainLayer.Enums;
using DomainLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Task = DomainLayer.Entities.Task;

namespace Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITaskGroupRepository _taskGroupRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly IProjectUserRepository _projectUserRepository;
    private readonly INotificationService _notificationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    public TaskService(ITaskRepository taskRepository, IMapper mapper, IUserRepository userRepository, ITaskGroupRepository taskGroupRepository, ISectionRepository sectionRepository, IProjectUserRepository projectUserRepository, INotificationService notificationService, IHttpContextAccessor httpContextAccessor)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
        _taskGroupRepository = taskGroupRepository;
        _sectionRepository = sectionRepository;
        _projectUserRepository = projectUserRepository;
        _notificationService = notificationService;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<TaskDto?> GetTaskByIdAsync(int id)
    {
        var task = await _taskRepository.GetByAsyncId(id);
        return  task == null ? null : _mapper.Map<TaskDto>(task);
    }

    public async Task<List<TaskDto>> GetAllTasksAsync(string email)
    {
        var tasks = await _taskRepository.GetAllAsync();
        return _mapper.Map<List<TaskDto>>(tasks);
    }

    public async Task<TaskDetailsDto> GetTaskDetailsAsync(int id)
    {
        var task = await _taskRepository.GetTaskWithDetailsAsync(id);

        var createdByName = task.CreatedBy.User.Name;
        var createdByInitial = task.CreatedBy.User.ProfileLetters;
        var createdByColor = task.CreatedBy.User.ProfileColor;


        var assignedToName = string.Empty;
        var assignedToInitial = string.Empty;
        var assignedToColor =  string.Empty;
        
        if (task.AssignedTo != null)
        {
            assignedToName = task.AssignedTo.User.Name;
            assignedToInitial = task.AssignedTo.User.ProfileLetters;
            assignedToColor= task.AssignedTo.User.ProfileColor;
        }

        var finishedByName = string.Empty;
        var finishedByInitial = string.Empty;
        var finishedByColor = string.Empty;
        
        if (task.FinishedBy != null)
        {
            finishedByName = task.FinishedBy.User.Name;
            finishedByInitial = task.FinishedBy.User.ProfileLetters;
            finishedByColor = task.FinishedBy.User.ProfileColor;
        }
        
        return new TaskDetailsDto
        {
            Title = task.Title,
            ListName = task.TaskGroup.Name,
            Description = task.Description,
            
            CreatedByName = createdByName,
            CreatedByInitial = createdByInitial,
            CreatedByColor = createdByColor,
            
            AssignedToName = assignedToName,
            AssignedInitial = assignedToInitial,
            AssignedColor = assignedToColor,
            
            FinishedByName = finishedByName,
            FinishedByInitial = finishedByInitial,
            FinishedByColor = finishedByColor,
            
            CreatedDate = task.StartDate.ToShortDateString(),
            DueDate = task.DueDate.HasValue ? task.DueDate.Value.ToShortDateString() : "",
            CompletedDate = task.CompletedDate.HasValue ? task.CompletedDate.Value.ToShortDateString() : "",
            
            Priority = task.Priority.ToString(),
            State = task.State.ToString()
        };
    }

    public async Task<TaskDto> SaveTaskAsync(int userId, string taskTitle, string taskGroupName, int sectionId)
    {
        var taskGroup = await _taskGroupRepository.FindFirstAsync(x => x.Name == taskGroupName && x.Section.Id == sectionId);
        var section = await _sectionRepository.GetSectionWithTasksAsync(sectionId);
        var createdBy = await _projectUserRepository.FindFirstAsync(x => x.UserId == userId && x.ProjectId == section!.ProjectId);

        var newTask = new Task 
        {
            Title = taskTitle,
            CreatedById = createdBy!.Id,
            TaskGroupId = taskGroup!.Id, 
            StartDate = DateTime.Now,
            Position = (taskGroup.Tasks?.Count ?? 0) + 1
        };

        await _taskRepository.AddAsync(newTask); 
    
        var newTaskDto = _mapper.Map<TaskDto>(newTask);
        return newTaskDto;
    }

    public async Task<int> ChangeTaskGroup(int taskId, int taskGroupId, int newPosition)
    {
        var currentTask = await _taskRepository.GetTaskWithDetailsAsync(taskId);
        var droppedTaskGroup = await _taskGroupRepository.GetByAsyncId(taskGroupId);

        if (droppedTaskGroup == null) return -1;

        var oldTaskList = currentTask.TaskGroup.Tasks;

        foreach (var task in oldTaskList!.Where(task => task.Position > currentTask.Position))
        {
            --task.Position;
            await _taskRepository.UpdateAsync(task);
        }

        currentTask.TaskGroup = droppedTaskGroup;
        currentTask.Position = newPosition;

        if (droppedTaskGroup.Tasks != null)
        {
            var newTaskList = droppedTaskGroup.Tasks;

            foreach (var task in newTaskList.Where(task => task.Position >= currentTask.Position && task.Id != droppedTaskGroup.Id))
            {
                task.Position++;
                await _taskRepository.UpdateAsync(task);
            }
        }
        
        await _taskRepository.UpdateAsync(currentTask);
        
        return droppedTaskGroup.Id;
    }

    public async Task<int> ChangeTaskPriority(int taskId, string priority)
    {
        var task = await _taskRepository.GetTaskWithDetailsAsync(taskId);
        var currentUserIdString = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (currentUserIdString == null) return -1;
        
        var currentUserId = int.Parse(currentUserIdString);
        
        var projectUser = await _projectUserRepository.FindFirstAsync(x => x.UserId == currentUserId && x.ProjectId == task.TaskGroup.Section.ProjectId);
        
        if (projectUser == null) return -1;
        
        if (projectUser.Role != ProjectRole.Leader && projectUser.Id != task.AssignedToId) return -1;

        task.Priority = priority switch
        {
            "low" => TaskPriority.Low,
            "medium" => TaskPriority.Medium,
            "high" => TaskPriority.High,
            _ => task.Priority
        };

        await _taskRepository.UpdateAsync(task);

        if (task.AssignedToId == null) return taskId;
        
        var userId = task.AssignedTo!.UserId;
        
        if (task.AssignedToId != null && userId != task.AssignedToId)
            await _notificationService.CreateNotificationAsync(userId, "Task Priority Update",
            $"Your assigned task {task.Title}'s priority has been updated: " + task.Priority, taskId, null, NotificationType.Info);

        return taskId;
    }

    public async Task<string> DeleteTask(int taskId, int userId, int projectId)
    {
        var projectUser = await _projectUserRepository.FindFirstAsync(x => x.UserId == userId && x.ProjectId == projectId);

        if (projectUser!.Role != ProjectRole.Leader) return "Task not deleted";
        
        var user = await _userRepository.GetByAsyncId(userId);
        if (user == null) return "Task not deleted";
        
        var task = await _taskRepository.GetTaskWithDetailsAsync(taskId);

        if (task.AssignedToId != null && task.AssignedToId != task.FinishedById)
        {
            if(task.AssignedTo == null)
                return "Task not deleted";
            
            task.AssignedTo.AssignedTaskCount -= 1;
            
            if(task.State != TaskState.Done)
                task.AssignedTo.PendingTaskCount -= 1;
            else
                task.AssignedTo.CompletedTaskCount -= 1;
            
            await _projectUserRepository.UpdateAsync(task.AssignedTo);
            
            if (task.AssignedTo.UserId != userId)
                await _notificationService.CreateNotificationAsync(task.AssignedTo.UserId, "Task Deletion",
                    $"Your task {task.Title} has been deleted by: " + user.Name, taskId, null, NotificationType.Info
                );
        }

        if (task.FinishedBy != null && task.AssignedToId != task.FinishedById)
        {
            task.FinishedBy.AssignedTaskCount -= 1;
            
            task.FinishedBy.CompletedTaskCount -= 1;
            
            await _projectUserRepository.UpdateAsync(task.FinishedBy);

            if (task.FinishedBy.UserId != userId)
                await _notificationService.CreateNotificationAsync(task.FinishedBy.UserId, "Task Deletion",
                    $"Your task {task.Title} has been deleted by: " + user.Name, taskId, null, NotificationType.Info
                );
        }

        if (task.AssignedToId == task.FinishedById && task is { AssignedToId: not null, FinishedById: not null })
        {
            task.FinishedBy!.AssignedTaskCount -= 1;
            
            task.FinishedBy.CompletedTaskCount -= 1;
            
            await _projectUserRepository.UpdateAsync(task.FinishedBy);

            if (task.FinishedBy.UserId != userId)
                await _notificationService.CreateNotificationAsync(task.FinishedBy.UserId, "Task Deletion",
                    $"Your task {task.Title} has been deleted by: " + user.Name, taskId, null, NotificationType.Info
                );
        }
        
        if (task.AssignedToId != null) task.AssignedToId = null;
        if (task.FinishedBy != null) task.FinishedBy = null;
        
        if (task.CreatedBy.UserId != userId)
            await _notificationService.CreateNotificationAsync(task.CreatedBy.UserId, "Task Deletion",
            $"Your task {task.Title} has been deleted by: " + user.Name, taskId, null, NotificationType.Info
            );
        
        await _taskRepository.DeleteAsync(taskId);
        
        return "Task deleted";
    }

    public async Task<int> ChangeTaskState(int taskId, int taskState, int userId, int projectId)
    {
        var projectUser = await _projectUserRepository.FindFirstAsync(x => x.UserId == userId && x.ProjectId == projectId);
        var task = await _taskRepository.GetTaskWithDetailsAsync(taskId);
        var user = await _userRepository.GetByAsyncId(userId);

        if (projectUser!.Role != ProjectRole.Leader && projectUser.Id != task.AssignedToId) return -1;

        task.State = taskState switch
        {
            0 => TaskState.Todo,
            1 => TaskState.InProgress,
            2 => TaskState.Done,
            _ => task.State
        };

        if (task is { State: TaskState.Done, FinishedBy: null })
        {
            var finishedByUser =
                await _projectUserRepository.FindFirstAsync(x => x.UserId == userId && x.ProjectId == projectId);
            if (finishedByUser == null) return -1;
            
            task.FinishedBy = finishedByUser;
            
            task.CompletedDate = DateTime.Now;

            finishedByUser.CompletedTaskCount += 1;
            await _projectUserRepository.UpdateAsync(finishedByUser);
        }
        else
        {
            var finishedByUser = task.FinishedBy;
            
            if (finishedByUser != null)
            {
                finishedByUser.CompletedTaskCount -= 1;
                finishedByUser.AssignedTaskCount -= 1;
                await _projectUserRepository.UpdateAsync(finishedByUser);
            }
            
            var assignedToUser = task.AssignedTo;
            
            if (assignedToUser != null)
            {
                assignedToUser.PendingTaskCount += 1;
                await _projectUserRepository.UpdateAsync(assignedToUser);
            }
        }
        
        await _taskRepository.UpdateAsync(task);
        
        if (task.AssignedToId != null && userId != task.AssignedToId)
        {
            await _notificationService.CreateNotificationAsync(task.AssignedTo!.UserId, "Task State Update",
                $"Your assigned task {task.Title}'s state has been updated by {user!.Name}: " + task.State, taskId, null, NotificationType.Info);
        }
        
        return taskId;
    }

    public async Task<int> ChangeTaskDescription(int userId, int taskId, int projectId, string description)
    {
        var task = await _taskRepository.GetTaskWithDetailsAsync(taskId);

        if (task.Description == description)
        {
            return taskId;
        }
        
        var projectUser = await _projectUserRepository.FindFirstAsync(x => x.UserId == userId && x.ProjectId == projectId);

        if (projectUser!.Role != ProjectRole.Leader && projectUser.Id != task.AssignedToId) return -1;

        task.Description = description;

        await _taskRepository.UpdateAsync(task);
        
        if (task.AssignedToId != null && userId != task.AssignedTo!.UserId)
        {
            await _notificationService.CreateNotificationAsync(task.AssignedTo!.UserId, "Task Description Update",
                "Your assigned task's description has been updated: " + task.Description, taskId, null, NotificationType.Info);
        }
        
        return taskId;
    }

    public async Task<int> ChangeTaskDueDate(int userId, int taskId, int projectId, string dueDate)
    {
        var projectUser = await _projectUserRepository.FindFirstAsync(x => x.UserId == userId && x.ProjectId == projectId);
        var task = await _taskRepository.GetTaskWithDetailsAsync(taskId);

        if (projectUser!.Role != ProjectRole.Leader && projectUser.Id != task.AssignedToId) return -1;
        
        var splitDate = dueDate.Split('-');
        var year = int.Parse(splitDate[0]);
        var month = int.Parse(splitDate[1]);
        var day = int.Parse(splitDate[2]);
        
        var date = new DateTime(year, month, day);
        task.DueDate = date;
        
        await _taskRepository.UpdateAsync(task);
        
        if (task.AssignedToId != null)
        {
            await _notificationService.CreateNotificationAsync(task.AssignedTo!.UserId, "Task Due Date Update",
                "Your assigned task's due date has been updated: " + task.DueDate, taskId, null, NotificationType.Info);
        }
        
        return taskId;
    }

    public async Task<int> AssignUserToTask(int userId, int taskId, int projectId, string userEmail)
    {
        var projectUser = await _projectUserRepository.FindFirstAsync(x => x.UserId == userId && x.ProjectId == projectId);
        if (projectUser!.Role != ProjectRole.Leader) return -1;

        if (userEmail == "Unassigned")
        {
            var taskToUnassign = await _taskRepository.GetTaskWithDetailsAsync(taskId);
            
            if (taskToUnassign.AssignedToId == null) return taskId;
            
            taskToUnassign.AssignedTo!.AssignedTaskCount -= 1;
            taskToUnassign.AssignedTo.PendingTaskCount -= 1;
            await _projectUserRepository.UpdateAsync(taskToUnassign.AssignedTo);
            
            taskToUnassign.AssignedToId = null;
            await _taskRepository.UpdateAsync(taskToUnassign);
            return taskId;
        }
        
        var user =  await _userRepository.FindFirstAsync(x => x.Email == userEmail);
        
        if (user == null)
        {
            Console.WriteLine("Could not find user with email: " + userEmail);
            return -1;
        }

        var assignUser =
            await _projectUserRepository.FindFirstAsync(x => x.User.Email == userEmail && x.ProjectId == projectId);

        if (assignUser == null)
        {
            Console.WriteLine("Could not find project user with email: " + userEmail + ", project id: " + projectId);
            return -1;
        }
        
        var task = await _taskRepository.GetTaskWithDetailsAsync(taskId);
        
        if (task.AssignedToId == assignUser.Id) return taskId;
        
        var currentAssignedProjectUser = task.AssignedTo;

        if (currentAssignedProjectUser != null && currentAssignedProjectUser.Id != assignUser.Id)
        {
            await _notificationService.CreateNotificationAsync(currentAssignedProjectUser.UserId, "Task Unassignment",
                "You have been unassigned from: " + task.Title, taskId, null, NotificationType.Info);
            
            currentAssignedProjectUser.AssignedTaskCount -= 1;
            currentAssignedProjectUser.PendingTaskCount -= 1;
            
            await _projectUserRepository.UpdateAsync(currentAssignedProjectUser);
        }

        task.AssignedToId = assignUser.Id;
        
        assignUser.AssignedTaskCount += 1;
        assignUser.PendingTaskCount += 1;
        
        await _projectUserRepository.UpdateAsync(assignUser);
        
        await _taskRepository.UpdateAsync(task);
        
        if (task.AssignedTo!.UserId != userId)
            await _notificationService.CreateNotificationAsync(assignUser.UserId, "Task Assigment",
                "You have been assigned to: " + task.Title, taskId, null, NotificationType.Info);
        
        return taskId;
    }

    public async Task<int> UpdateTitle(int taskId, string title, int userId, int projectId)
    {
        var projectUser = await _projectUserRepository.FindFirstAsync(x => x.UserId == userId && x.ProjectId == projectId);
        var task = await _taskRepository.GetTaskWithDetailsAsync(taskId);
        
        if (task.Title == title) return taskId;

        if (projectUser!.Role != ProjectRole.Leader && projectUser.Id != task.AssignedToId) return -1;

        task.Title = title;
        await _taskRepository.UpdateAsync(task);

        if (task.AssignedToId != null)
        {
            await _notificationService.CreateNotificationAsync(projectUser.UserId, "Task Title Update",
                "Your assigned task's title has been updated: " + task.Title, taskId, null, NotificationType.Info);
        }
        
        return taskId;
    }
}