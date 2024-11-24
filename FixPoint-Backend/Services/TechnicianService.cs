using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using FixPoint_Backend.Models;

namespace FixPoint_Backend.Services;

public class TechnicianService
{
    private readonly ITechnicianRepository _technicianRepository;
    private readonly ILogger<TechnicianController> _logger;
    
    public TechnicianService(ITechnicianRepository technicianRepository, ILogger<TechnicianController> logger)
    {
        _technicianRepository = technicianRepository;
        _logger = logger;
    }
    
    
    public Technician GetTechnician(Guid id)
    {
        _logger.LogInformation("Getting a technician by id");
        return _technicianRepository.GetTechnician(id);
    }
    
    public List<Technician> GetTechnicians()
    {
        _logger.LogInformation("Getting all technicians");
        return _technicianRepository.GetTechnicians();
    }
}