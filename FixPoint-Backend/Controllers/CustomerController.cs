using FixPoint_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using FixPoint_Backend.Services;
using FixPoint_Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FixPoint_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;
    
    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }
    
    [HttpPost]
    public IActionResult AddCustomer([FromBody] Customer customer)
    {
        _customerService.AddCustomer(customer);
        return Ok("Customer: "+customer.GetID().ToString() + " added");
    }
    
    [HttpDelete("[action]")]
    public IActionResult DeleteCustomer([FromBody] Customer customer)
    {
        _customerService.DeleteCustomer(customer);
        return Ok("Customer: "+customer.GetID().ToString() + " deleted");
    }
    
    [HttpGet("[action]")]
    public IActionResult GetCustomerById(Guid id)
    {
        Customer c = _customerService.GetCustomer(id);
        if ( c == null || c.GetID() != id)
        {
            return NotFound("Customer not found");
        }
        return Ok("Customer: "+id.ToString() + " retrieved");
    }
    
    [HttpGet("[action]")]
    public IActionResult GetCustomers()
    {
        List<Customer> clist =  _customerService.GetCustomers();
        if (clist.Count == 0)
        {
            return NotFound("No customers found");
        }
        return Ok("All customers retrieved");
    }
}