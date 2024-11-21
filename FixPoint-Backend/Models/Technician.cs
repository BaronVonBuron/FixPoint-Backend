namespace FixPoint_Backend.Models;

public class Technician
{
    Guid ID { get; set; }
    string Name { get; set; }
    string Email { get; set; }
    string Salt { get; set; }
    string Password { get; set; }
    
    
    public Technician(string name, string email, string salt, string password)
    {
        ID = Guid.NewGuid();
        Name = name;
        Email = email;
        Salt = salt;
        Password = password;
    }
    
    
}