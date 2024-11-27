using FixPoint_Backend.Models;

namespace FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;

public interface ICaseRepository
{
    void AddCase(Case casee);
    void DeleteCase(Case casee);
    Case GetCase(Guid id);
    List<Case> GetCases();
    void UpdateCase(Case casee);
}