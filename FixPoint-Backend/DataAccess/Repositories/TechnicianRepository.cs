using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;

namespace FixPoint_Backend.DataAccess.Repositories;

public class TechnicianRepository : ITechnicianRepository
{
    private readonly AppDbContext _context;
    
    public TechnicianRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public void AddTechnician(Technician technician)
    {
        _context.Technicians.Add(technician);
        _context.SaveChanges();
    }
    public Technician GetTechnician(Guid id)
    {
        return _context.Technicians.Find(id);
    }
    
    public List<Technician> GetTechnicians()
    {
        return _context.Technicians.ToList();
    }
    
    
}