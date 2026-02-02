using DomainLayer.Enums;

namespace DomainLayer.Entities;

public class Notification
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public virtual User User { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    
    public required DateTime CreatedDate { get; set; }
    
    public int? RelatedTaskId { get; set; }
    public int? RelatedEntityId { get; set; }
    
    public bool IsRead { get; set; }
    
    public NotificationType Type { get; set; }
}