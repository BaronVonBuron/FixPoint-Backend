using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;

namespace FixPoint_Backend.DataAccess.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;
    
    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public void UpdateCustomer(Customer customer)
    {
        var existingCustomer = _context.Customers.Find(customer.ID);
        if (existingCustomer != null)
        {
            // Update fields
            existingCustomer.Name = customer.Name;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phonenumber = customer.Phonenumber;
            existingCustomer.CPRCVR = customer.CPRCVR;

            // Save changes
            _context.SaveChanges();
        }
    }
    
    public void AddCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
    }
    
    public void DeleteCustomer(Customer customer)
    {
        _context.Customers.Remove(customer);
        _context.SaveChanges();
    }
    
    public Customer GetCustomer(Guid id)
    {
        return _context.Customers.Find(id);
    }
    
    public List<Customer> GetCustomers()
    {
        return _context.Customers.ToList();
    }
    
    
}