namespace FixPoint_Backend.DataAccess;

public class LoginDTO
{
    
    public string Username { get; set; }
    public string Password { get; set; }

    public LoginDTO(string username, string password)
    {
        Username = username;
        Password = password;
    }

    // Parameterless constructor for serialization/deserialization
    public LoginDTO()
    {
    }
    
}