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
        return Ok("Case: "+casee.GetID().ToString() + " added");
    }
    
    [HttpDelete("[action]")]
    public IActionResult DeleteCase([FromBody] Case casee)
    {
        _caseeService.DeleteCase(casee);
        return Ok("Case: "+casee.GetID().ToString() + " deleted");
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
        return Ok("All casees retrieved");
    }
    
    [HttpPut("[action]")]
    public IActionResult UpdateCase([FromBody] Case casee)
    {
        _caseeService.UpdateCase(casee);
        return Ok("Case: "+casee.GetID().ToString() + " updated");
    }
}