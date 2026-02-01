using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TaskManagementSystem.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    
}