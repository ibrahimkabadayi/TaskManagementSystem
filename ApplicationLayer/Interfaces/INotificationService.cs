using Application.DTOs;
using DomainLayer.Enums;

namespace Application.Interfaces;

public interface INotificationService
{
    Task CreateNotificationAsync(int userId, string title, string message, int? relatedTaskId, NotificationType type);
    Task<List<NotificationDto>> GetUnreadNotificationsAsync(int userId);
    Task MarkAsReadAsync(int notificationId);
}