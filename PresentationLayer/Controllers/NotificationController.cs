using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementSystem.Controllers;

public class NotificationController : Controller
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUnreadNotifications()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

        var userId = int.Parse(userIdString);

        var notifications = await _notificationService.GetUnreadNotificationsAsync(userId);
        
        var sortedNotifications = notifications.OrderByDescending(n => n.CreatedDate).ToList();

        return Json(sortedNotifications);
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead([FromQuery] int notificationId)
    {
        await _notificationService.MarkAsReadAsync(notificationId);
        return Ok();
    }
}