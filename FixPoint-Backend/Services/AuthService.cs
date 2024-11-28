using System.Security.Cryptography;
using System.Text;
using FixPoint_Backend.DataAccess;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Services.ServiceInterface;

namespace FixPoint_Backend.Services;

public class AuthService : IAuthService
{
    private readonly ITechnicianRepository _technicianRepository;
    private readonly ICustomerRepository _customerRepository;

    public AuthService(ITechnicianRepository technicianRepository, ICustomerRepository customerRepository)
    {
        _technicianRepository = technicianRepository;
        _customerRepository = customerRepository;
    }

    public string Login(LoginDTO loginDto)
    {
        // Check for null or empty Username and Password
        if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
        {
            return null;
        }

        if (loginDto.Username.Contains("@")) // Technician login
        {
            var technician = _technicianRepository.GetTechnicians()
                .FirstOrDefault(t => t.Email == loginDto.Username);
            if (technician == null) return null;

            var hashedPassword = HashPassword(loginDto.Password, technician.Salt);
            if (hashedPassword != technician.Password) return null;

            return "mockedToken"; // Replace with JWT generation.
        }

        // Customer login
        var customer = _customerRepository.GetCustomers()
            .FirstOrDefault(c => c.Phonenumber == loginDto.Username && c.CPRCVR == loginDto.Password);
        if (customer == null) return null;

        return "mockedToken"; // Replace with JWT generation.
    }

    public string HashPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = password + salt;
            var bytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}