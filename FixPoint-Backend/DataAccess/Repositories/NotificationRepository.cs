using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;

namespace FixPoint_Backend.DataAccess.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;
    
    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public void AddNotification(Notification notification)
    {
        _context.Notifications.Add(notification);
        _context.SaveChanges();
    }
    
    public Notification GetNotification(Guid id)
    {
        return _context.Notifications.Find(id);
    }
    
    public List<Notification> GetNotifications()
    {
        return _context.Notifications.ToList();
    }
    
    
}