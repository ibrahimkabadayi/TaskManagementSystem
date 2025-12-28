using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;

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

        var section = await _sectionRepository.GetByAsyncId(sectionId);

        var newTaskGroup = new TaskGroup
        {
            Section = section!,
            CreatedBy = createdBy,
            Name = taskGroupName
        };

        await _taskGroupRepository.AddAsync(newTaskGroup);
        
        return _mapper.Map<TaskGroupDto>(newTaskGroup);
    }
}