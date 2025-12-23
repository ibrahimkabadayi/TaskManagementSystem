using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Implemantations;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer.Repositories.Implementations;

public class TaskGroupRepository : Repository<TaskGroup>, ITaskGroupRepository
{
    public TaskGroupRepository(ApplicationDbContext context) : base(context)
    {
    }
}