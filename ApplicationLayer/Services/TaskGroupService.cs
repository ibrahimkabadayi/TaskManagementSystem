using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class TaskGroupService : ITaskGroupService
{
    private readonly ITaskGroupRepository _taskGroupRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectUserRepository _projectUserRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly IMapper _mapper;

    public TaskGroupService(ITaskGroupRepository taskGroupRepository,  ITaskRepository taskRepository, IProjectUserRepository projectUserRepository, ISectionRepository sectionRepository, IMapper mapper)
    {
        _taskGroupRepository = taskGroupRepository;
        _taskRepository = taskRepository;
        _projectUserRepository = projectUserRepository;
        _sectionRepository = sectionRepository;
        _mapper = mapper;
    }
    public async Task<TaskGroupDto?> GetSectionByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TaskGroupDto>> GetAllSectionsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<TaskGroupDto?> SaveTaskGroupAsync(string taskGroupName, int sectionId, int userId)
    {
        var createdBy = await _projectUserRepository.FindFirstAsync(x => x.UserId == userId && x.Project.Sections.Any(s => s.Id == sectionId));
        if (createdBy is null)
            return null;

        var section = await _sectionRepository.GetSectionWithTasksAsync(sectionId);

        var newTaskGroup = new TaskGroup
        {
            SectionId = section!.Id,
            CreatedById = createdBy.Id,
            Name = taskGroupName
        };

        await _taskGroupRepository.AddAsync(newTaskGroup);
        
        return _mapper.Map<TaskGroupDto>(newTaskGroup);
    }

    public async Task<TaskGroupDto> ChangeTaskGroupNameAsync(int id, string newTaskGroupName)
    {
        var taskGroup = await _taskGroupRepository.GetByAsyncId(id);
        taskGroup!.Name = newTaskGroupName;
        await _taskGroupRepository.UpdateAsync(taskGroup);
        return _mapper.Map<TaskGroupDto>(taskGroup);
    }

    public async Task DeleteTaskGroupAsync(int id)
    {
        await _taskGroupRepository.DeleteAsync(id);
    }

    public async Task<List<TaskDto>> GetAllTasksAsync(int id)
    {
        var taskGroup = await _taskGroupRepository.GetByAsyncId(id);
        return taskGroup!.Tasks!.Select(task => _mapper.Map<TaskDto>(task)).ToList();
    }
}