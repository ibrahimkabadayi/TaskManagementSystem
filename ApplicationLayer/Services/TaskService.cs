using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DomainLayer.Enums;
using DomainLayer.Interfaces;
using Task = DomainLayer.Entities.Task;

namespace Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITaskGroupRepository _taskGroupRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly IProjectUserRepository _projectUserRepository;
    private readonly IMapper _mapper;
    public TaskService(ITaskRepository taskRepository, IMapper mapper, IUserRepository userRepository, ITaskGroupRepository taskGroupRepository, ISectionRepository sectionRepository, IProjectUserRepository projectUserRepository)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
        _taskGroupRepository = taskGroupRepository;
        _sectionRepository = sectionRepository;
        _projectUserRepository = projectUserRepository;
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
        var task = await _taskRepository.GetByAsyncId(id);

        var createdByName = task!.CreatedBy.User.Name;
        var createdByArray = createdByName.Split(' ');
        var createdByInitial = (createdByArray.Length == 2) ? createdByName.Split(' ')[0][0] + "" + createdByName.Split(' ')[1][0] : createdByArray[0][0].ToString();
        var createdByColor = task.CreatedBy.User.ProfileColor;
        
        var assignedToName = task.AssignedTo!.User.Name;
        var assignedToArray = assignedToName.Split(' ');
        var assignedToInitial = (assignedToArray.Length == 2) ? assignedToName.Split(' ')[0][0] + "" + assignedToName.Split(' ')[1][0] : assignedToArray[0][0].ToString();
        var assignedToColor = task.AssignedTo.User.ProfileColor;
        
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
            CreatedDate = task.StartDate.ToShortDateString(),
            DueDate = task.DueDate.HasValue ? task.DueDate.Value.ToShortDateString() : "",
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
        var currentTask = await _taskRepository.GetByAsyncId(taskId);
        var droppedTaskGroup = await _taskGroupRepository.GetByAsyncId(taskGroupId);

        var oldTaskList = currentTask!.TaskGroup.Tasks;

        foreach (var task in oldTaskList!.Where(task => task.Position > currentTask.Position))
        {
            --task.Position;
            await _taskRepository.UpdateAsync(task);
        }

        currentTask.TaskGroup = droppedTaskGroup!;
        currentTask.Position = newPosition;

        var newTaskList = droppedTaskGroup!.Tasks;

        foreach (var task in newTaskList!.Where(task => task.Position >= currentTask.Position))
        {
            task.Position++;
            await _taskRepository.UpdateAsync(task);
        }
        
        await _taskRepository.UpdateAsync(currentTask);
        return droppedTaskGroup.Id;
    }

    public async Task<int> ChangeTaskPriority(int taskId, string priority)
    {
        var task = await _taskRepository.GetByAsyncId(taskId);

        task!.Priority = priority switch
        {
            "🟢 Low Priority" => TaskPriority.Low,
            "🟡 Medium Priority" => TaskPriority.Medium,
            "🔴 High Priority" => TaskPriority.High,
            _ => task!.Priority
        };

        await _taskRepository.UpdateAsync(task);
        
        return taskId;
    }

    public async Task<string> DeleteTask(int taskId, int userId, int projectId)
    {
        var projectUser = await _projectUserRepository.FindFirstAsync(x => x.UserId == userId && x.ProjectId == projectId);

        if (projectUser!.Role != ProjectRole.Leader) return "Task not deleted";
        
        await _taskRepository.DeleteAsync(taskId);
        
        return "Task deleted";
    }
}