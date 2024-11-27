using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using FixPoint_Backend.Models;

namespace FixPoint_Backend.Services;

public class NotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ILogger<NotificationController> _logger;
    
    public NotificationService(INotificationRepository notificationRepository, ILogger<NotificationController> logger)
    {
        _notificationRepository = notificationRepository;
        _logger = logger;
    }
    
    public void AddNotification(Notification notification)
    {
        if (notification == null)
        {
            nullCaseException(notification);
        }
        _logger.LogInformation("Adding a new notification");
        _notificationRepository.AddNotification(notification);
    }
    
    public Notification GetNotification(Guid id)
    {
        _logger.LogInformation("Getting a notification by id");
        return _notificationRepository.GetNotification(id);
    }
    
    public List<Notification> GetNotifications()
    {
        _logger.LogInformation("Getting all notifications");
        return _notificationRepository.GetNotifications();
    }
    
    public void nullCaseException(Notification notification)
    {
        _logger.LogError("Case is null");
        throw new ArgumentNullException(nameof(notification));
    }
}