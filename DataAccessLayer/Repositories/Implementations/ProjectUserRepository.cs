using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Implemantations;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer.Repositories.Implementations;

public class ProjectUserRepository : Repository<ProjectUser>, IProjectUserRepository
{
    public ProjectUserRepository(ApplicationDbContext context) : base(context)
    {
    }
}