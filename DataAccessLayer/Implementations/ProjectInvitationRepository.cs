using DataAccessLayer.Context;
using DomainLayer.Entities;
using DomainLayer.Interfaces;

namespace DataAccessLayer.Implementations;

public class ProjectInvitationRepository : Repository<ProjectInvitation>, IProjectInvitationRepository
{
    public ProjectInvitationRepository(ApplicationDbContext context) : base(context)
    {
    }
}