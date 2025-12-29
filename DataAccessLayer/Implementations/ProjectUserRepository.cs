using DataAccessLayer.Context;
using DomainLayer.Entities;
using DomainLayer.Interfaces;

namespace DataAccessLayer.Implementations;

public class ProjectUserRepository : Repository<ProjectUser>, IProjectUserRepository
{
    public ProjectUserRepository(ApplicationDbContext context) : base(context)
    {
    }
}