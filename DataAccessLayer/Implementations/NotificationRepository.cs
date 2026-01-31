using DataAccessLayer.Context;
using DomainLayer.Entities;
using DomainLayer.Interfaces;

namespace DataAccessLayer.Implementations;

public class NotificationRepository : Repository<Notification>, INotificationRepository 
{
    public NotificationRepository(ApplicationDbContext context) : base(context)
    {
        
    }
    
}