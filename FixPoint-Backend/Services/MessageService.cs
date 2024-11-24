using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using FixPoint_Backend.Models;

namespace FixPoint_Backend.Services;

public class MessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly ILogger<MessageController> _logger;
    
    public MessageService(IMessageRepository messageRepository, ILogger<MessageController> logger)
    {
        _messageRepository = messageRepository;
        _logger = logger;
    }
    
    public void AddMessage(Message message)
    {
        _logger.LogInformation("Adding a new message");
        _messageRepository.AddMessage(message);
    }
    
    public Message GetMessage(Guid id)
    {
        _logger.LogInformation("Getting a message by id");
        return _messageRepository.GetMessage(id);
    }
    
    public List<Message> GetMessages()
    {
        _logger.LogInformation("Getting all messages");
        return _messageRepository.GetMessages();
    }
}