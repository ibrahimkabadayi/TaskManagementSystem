namespace Application.Interfaces;

public interface INotifier
{
    Task SendNotificationAsync(string userId, string title, string message);
}