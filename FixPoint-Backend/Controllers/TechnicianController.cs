using FixPoint_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using FixPoint_Backend.Models;
using FixPoint_Backend.Services.ServiceInterface;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FixPoint_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TechnicianController : ControllerBase
{
    private readonly TechnicianService _technicianService;
    private readonly IAuthService _authService;

    public TechnicianController(TechnicianService technicianService, IAuthService authService)
    {
        _technicianService = technicianService;
        _authService = authService;
    }

    [HttpPost]
    public IActionResult AddTechnician([FromBody] TechnicianInputModel technicianInput)
    {
        // Generate salt
        byte[] salt = new byte[16];
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        string saltString = Convert.ToBase64String(salt);

        // Hash the password with the salt using AuthService
        string hashedPassword = _authService.HashPassword(technicianInput.Password, saltString);

        // Create Technician object
        Technician technician = new Technician
        {
            Name = technicianInput.Name,
            Email = technicianInput.Email,
            Password = hashedPassword,
            Salt = saltString
        };

        _technicianService.AddTechnician(technician);
        return Ok(technician);
    }

    [HttpGet("[action]")]
    public IActionResult GetTechnicianById(Guid id)
    {
        Technician t = _technicianService.GetTechnician(id);
        if (t == null || t.ID != id)
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
        return Ok(tlist);
    }
}