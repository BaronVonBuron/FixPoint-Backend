using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;

namespace FixPoint_Backend.DataAccess.Repositories;

public class CaseRepository : ICaseRepository
{
    private readonly AppDbContext _context;
    
    public CaseRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public void AddCase(Case casee)
    {
        _context.Cases.Add(casee);
        _context.SaveChanges();
    }
    
    public List<Case> GetCasesByCustomer(Guid customerId)
    {
        return _context.Cases.Where(c => c.CustomerFK == customerId).ToList();
    }
    
    public void DeleteCase(Case casee)
    {
        _context.Cases.Remove(casee);
        _context.SaveChanges();
    }
    
    public Case GetCase(Guid id)
    {
        return _context.Cases.Find(id);
    }
    
    public List<Case> GetCases()
    {
        return _context.Cases.ToList();
    }

    public void UpdateCase(Case casee)
    {
        _context.Cases.Update(casee);
    }
}