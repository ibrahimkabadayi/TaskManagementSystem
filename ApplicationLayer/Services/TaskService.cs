using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DataAccessLayer.Repositories.Interfaces;

namespace Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;
    public TaskService(ITaskRepository taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
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
}