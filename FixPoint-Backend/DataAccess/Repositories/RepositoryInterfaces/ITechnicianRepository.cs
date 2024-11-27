using FixPoint_Backend.Models;

namespace FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;

public interface ITechnicianRepository
{
    Technician GetTechnician(Guid id);
    List<Technician> GetTechnicians();
}