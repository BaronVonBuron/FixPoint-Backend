using FixPoint_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using FixPoint_Backend.Services;
using FixPoint_Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FixPoint_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TechnicianController : ControllerBase
{
    private readonly TechnicianService _technicianService;
    
    public TechnicianController(TechnicianService technicianService)
    {
        _technicianService = technicianService;
    }
    
    [HttpGet("[action]")]
    public IActionResult GetTechnicianById(Guid id)
    {
        Technician t = _technicianService.GetTechnician(id);
        if ( t == null || t.ID != id)
        {
            return NotFound("Technician not found");
        }
        return Ok(t);
    }
    
    [HttpGet("[action]")]
    public IActionResult GetTechnicians()
    {
        List<Technician> tlist = _technicianService.GetTechnicians();
        if (tlist.Count == 0)
        {
            return NotFound("No technicians found");
        }
        return Ok("Technicians retrieved");
    }
}