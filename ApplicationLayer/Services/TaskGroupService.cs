using Application.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;

namespace Application.Services;

public class TaskGroupService : ITaskGroupService
{
    public async Task<TaskGroup?> GetSectionByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TaskGroup>> GetAllSectionsAsync()
    {
        throw new NotImplementedException();
    }
}