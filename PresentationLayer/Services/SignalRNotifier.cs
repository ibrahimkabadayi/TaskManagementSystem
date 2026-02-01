using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using TaskManagementSystem.Hubs;

namespace TaskManagementSystem.Services;

public class SignalRNotifier : INotifier
{
    private readonly IHubContext<NotificationHub> _hubContext;
    
    public SignalRNotifier(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public async Task SendNotificationAsync(string userId, string title, string message)
    {
        await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", title, message);
    }
}