namespace FixPoint_Backend.Models;

public class Customer
{
    Guid ID { get; set; }
    string Name { get; set; }
    string Email { get; set; }
    string Phone { get; set; }
    string CPRCVR { get; set; }
    
    public Customer(string name, string email, string phone, string cprcvr)
    {
        ID = Guid.NewGuid();
        Name = name;
        Email = email;
        Phone = phone;
        CPRCVR = cprcvr;
    }
}