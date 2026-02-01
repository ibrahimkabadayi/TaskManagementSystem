using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Enums;
using DomainLayer.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotifier _notifier;
    private readonly IMapper _mapper;

    public NotificationService(INotificationRepository notificationRepository, INotifier notifier, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _notifier = notifier;
        _mapper = mapper;
    }
    
    public async Task CreateNotificationAsync(int userId, string title, string message, int? relatedTaskId, NotificationType type)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            RelatedTaskId = relatedTaskId,
            Type = type,
            IsRead = false,
            CreatedDate = DateTime.Now
        };

        await _notifier.SendNotificationAsync(userId.ToString(), title, message);

        await _notificationRepository.AddAsync(notification);
    }

    public async Task<List<NotificationDto>> GetUnreadNotificationsAsync(int userId)
    {
        var notifications = await _notificationRepository.FindAsync(n => n.UserId == userId && !n.IsRead);

        return _mapper.Map<List<NotificationDto>>(notifications);
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        var notification = await _notificationRepository.GetByAsyncId(notificationId);

        if (notification != null)
        {
            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
        }
    }
}