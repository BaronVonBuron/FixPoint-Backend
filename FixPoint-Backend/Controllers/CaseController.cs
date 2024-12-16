using FixPoint_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using FixPoint_Backend.Services;
using FixPoint_Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FixPoint_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CaseController : ControllerBase
{
    private readonly CaseService _caseeService;
    
    public CaseController(CaseService caseeService)
    {
        _caseeService = caseeService;
    }
    
    [HttpPost]
    public IActionResult AddCase([FromBody] Case casee)
    {
        _caseeService.AddCase(casee);
        return Ok(casee);
    }
    
    [HttpGet("GetByCustomer")]
    public IActionResult GetCasesByCustomer(string customerId)
    {
        if (string.IsNullOrEmpty(customerId))
        {
            return BadRequest("Customer ID is required.");
        }

        var cases = _caseeService.GetCasesByCustomer(customerId);
        if (cases == null || cases.Count == 0)
        {
            return NotFound("No cases found for the specified customer.");
        }

        return Ok(cases);
    }
    
    [HttpDelete("[action]")]
    public IActionResult DeleteCase(Guid caseId)
    {
        var casee = _caseeService.GetCase(caseId);
        if (casee == null)
        {
            return NotFound(new { message = $"Case with ID {caseId} not found." });
        }

        _caseeService.DeleteCase(caseId);
        return Ok(new { message = $"Sag med ID: {caseId} slettet." });
    }
    
    [HttpGet("[action]")]
    public IActionResult GetCaseById(Guid id)
    {
        Case c = _caseeService.GetCase(id);
        if ( c == null || c.GetID() != id)
        {
            return NotFound("Case not found");
        }
        return Ok("Case: "+id.ToString() + " retrieved");
    }
    
    [HttpGet("[action]")]
    public IActionResult GetCases()
    {
        List<Case> clist = _caseeService.GetCases();
        if (clist.Count == 0)
        {
            return NotFound("No cases found");
        }
        return Ok(clist);
    }
    
    [HttpPut("[action]")]
    public IActionResult UpdateCase([FromBody] Case casee)
    {
        _caseeService.UpdateCase(casee);

        // Return a JSON object with a message and any relevant data
        return Ok(new
        {
            message = $"Case {casee.GetID()} updated successfully",
            caseId = casee.GetID()
        });
    }
}