using DomainLayer.Enums;

namespace Application.DTOs;

public class NotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool IsRead { get; set; }
    public int? RelatedTaskId { get; set; }
    public int? RelatedEntityId { get; set; }
    public NotificationType Type { get; set; }
    public required UserDto User { get; set; }
}