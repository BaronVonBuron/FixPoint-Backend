using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;

namespace FixPoint_Backend.DataAccess.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;
    
    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public List<Message> GetMessagesByCaseId(Guid caseId)
    {
        return _context.Messages.Where(m => m.CaseFK == caseId).ToList();
    }
    
    public void AddMessage(Message message)
    {
        _context.Messages.Add(message);
        _context.SaveChanges();
    }
    
    public Message GetMessage(Guid id)
    {
        return _context.Messages.Find(id);
    }
    
    public List<Message> GetMessages()
    {
        return _context.Messages.ToList();
    }
    
    
}