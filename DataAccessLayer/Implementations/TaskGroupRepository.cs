using DataAccessLayer.Context;
using DomainLayer.Entities;
using DomainLayer.Interfaces;

namespace DataAccessLayer.Implementations;

public class TaskGroupRepository : Repository<TaskGroup>, ITaskGroupRepository
{
    public TaskGroupRepository(ApplicationDbContext context) : base(context)
    {
    }
}