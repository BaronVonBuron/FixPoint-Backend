using FixPoint_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using FixPoint_Backend.Services;
using FixPoint_Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FixPoint_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class NotificationController : ControllerBase
{
    private readonly NotificationService _notificationService;
    
    public NotificationController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    [HttpPost]
    public IActionResult AddNotification([FromBody] Notification notification)
    {
        _notificationService.AddNotification(notification);
        return Ok("Notification: "+notification.GetID().ToString() + " added");
    }
    
    [HttpGet("[action]")]
    public IActionResult GetNotificationById(Guid id)
    {
        Notification n = _notificationService.GetNotification(id);
        if ( n == null || n.GetID() != id)
        {
            return NotFound("Notification not found");
        }
        
        return Ok("Notification: "+id.ToString() + " retrieved");
    }
    
    [HttpGet("[action]")]
    public IActionResult GetNotifications()
    {
        List<Notification> nlist = _notificationService.GetNotifications();
        if (nlist.Count == 0)
        {
            return NotFound("No notifications found");
        }
        return Ok("All notifications retrieved");
    }
}