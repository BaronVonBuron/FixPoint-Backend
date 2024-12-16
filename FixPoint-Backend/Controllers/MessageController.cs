using FixPoint_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using FixPoint_Backend.Services;
using FixPoint_Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FixPoint_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class MessageController : ControllerBase
{
    private readonly MessageService _messageService;
    
    public MessageController(MessageService messageService)
    {
        _messageService = messageService;
    }
    
    [HttpGet("[action]")]
    public IActionResult GetMessagesByCaseId(Guid caseId)
    {
        List<Message> messages = _messageService.GetMessagesByCaseId(caseId);

        if (messages == null || messages.Count == 0)
        {
            return NotFound($"No messages found for case ID: {caseId}");
        }

        return Ok(messages);
    }
    
    [HttpPost]
    public IActionResult AddMessage([FromBody] Message message)
    {
        _messageService.AddMessage(message);
        return Ok("Message: "+message.GetID().ToString() + " added");
    }
    
    [HttpGet("[action]")]
    public IActionResult GetMessageById(Guid id)
    {
        Message m = _messageService.GetMessage(id);
        if ( m == null || m.GetID() != id)
        {
            return NotFound("Message not found");
        }
        return Ok("Message: "+id.ToString() + " retrieved");
    }
    
    [HttpGet("[action]")]
    public IActionResult GetMessages()
    {
        List<Message> mlist = _messageService.GetMessages();
        if (mlist.Count == 0)
        {
            return NotFound("No messages found");
        }
        return Ok("All messages retrieved");
    }
}