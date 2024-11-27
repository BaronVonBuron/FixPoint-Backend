using System.Dynamic;

namespace FixPoint_Backend.Models;

public class Customer
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phonenumber { get; set; }
    public string CPRCVR { get; set; }
    
    // Parameterized constructor
    public Customer(string name, string email, string phone, string cprcvr)
    {
        ID = Guid.NewGuid();
        Name = name;
        Email = email;
        Phonenumber = phone;
        CPRCVR = cprcvr;
    }
    
    // Parameterless constructor for EF Core
    public Customer()
    {
        ID = Guid.NewGuid();
    }

    public Guid GetID()
    {
        return ID;
    }
}