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
        if (casee == null)
        {
            nullCaseException(casee);
        }
        _logger.LogInformation("Adding a new casee");
        _caseeRepository.AddCase(casee);
    }
    
    public List<Case> GetCasesByCustomer(string customerId)
    {
        if (string.IsNullOrEmpty(customerId))
        {
            throw new ArgumentException("Customer ID is required", nameof(customerId));
        }

        _logger.LogInformation($"Getting cases by customer with ID: {customerId}");
        if (!Guid.TryParse(customerId, out var customerGuid))
        {
            throw new ArgumentException("Invalid customer ID format", nameof(customerId));
        }

        return _caseeRepository.GetCasesByCustomer(customerGuid);
    }
    
    public void DeleteCase(Guid caseId)
    {
        var casee = _caseeRepository.GetCase(caseId);
        if (casee == null)
        {
            _logger.LogError($"Case with ID {caseId} not found.");
            throw new KeyNotFoundException("Case not found.");
        }
    
        _logger.LogInformation($"Deleting case with ID {caseId}");
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
        if (casee == null)
        {
            nullCaseException(casee);
        }
        _logger.LogInformation("Updating a casee");
        _caseeRepository.UpdateCase(casee);
    }

    public void nullCaseException(Case casee)
    {
        _logger.LogError("Case is null");
        throw new ArgumentNullException(nameof(casee));
    }
}