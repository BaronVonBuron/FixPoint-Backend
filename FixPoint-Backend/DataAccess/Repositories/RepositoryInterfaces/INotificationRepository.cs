using FixPoint_Backend.Models;

namespace FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;

public interface INotificationRepository
{
    void AddNotification(Notification notification);
    Notification GetNotification(Guid id);
    List<Notification> GetNotifications();
}