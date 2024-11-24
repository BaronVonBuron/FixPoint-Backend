using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using FixPoint_Backend.Models;

namespace FixPoint_Backend.Services;

public class CaseService
{
    private readonly ICaseRepository _caseeRepository;
    private readonly ILogger<CaseController> _logger;
    
    public CaseService(ICaseRepository caseeRepository, ILogger<CaseController> logger)
    {
        _caseeRepository = caseeRepository;
        _logger = logger;
    }
    
    public void AddCase(Case casee)
    {
        _logger.LogInformation("Adding a new casee");
        _caseeRepository.AddCase(casee);
    }
    
    public void DeleteCase(Case casee)
    {
        _logger.LogInformation("Deleting a casee");
        _caseeRepository.DeleteCase(casee);
    }
    
    public Case GetCase(Guid id)
    {
        _logger.LogInformation("Getting a casee by id");
        return _caseeRepository.GetCase(id);
    }
    
    public List<Case> GetCases()
    {
        _logger.LogInformation("Getting all casees");
        return _caseeRepository.GetCases();
    }
    
    public void UpdateCase(Case casee)
    {
        _logger.LogInformation("Updating a casee");
        _caseeRepository.UpdateCase(casee);
    }
}