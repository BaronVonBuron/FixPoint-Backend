namespace FixPoint_Backend.Models;

public class Technician
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Salt { get; set; }
    public string Password { get; set; }
    
    
    public Technician(string name, string email, string salt, string password)
    {
        ID = Guid.NewGuid();
        Name = name;
        Email = email;
        Salt = salt;
        Password = password;
    }
    
    public Technician()
    {
        
    }
    
    public Guid GetID()
    {
        return ID;
    }
    
}