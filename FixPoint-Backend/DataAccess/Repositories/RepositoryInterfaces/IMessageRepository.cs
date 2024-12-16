using FixPoint_Backend.Models;

namespace FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;

public interface IMessageRepository
{
    void AddMessage(Message message);
    Message GetMessage(Guid id);
    List<Message> GetMessages();
    List<Message> GetMessagesByCaseId(Guid caseId);
}