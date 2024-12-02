using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FixPoint_Backend.DataAccess;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Services.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FixPoint_Backend.Services;

[Authorize]
public class AuthService : IAuthService
{
    private readonly ITechnicianRepository _technicianRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public AuthService(
            ITechnicianRepository technicianRepository,
            ICustomerRepository customerRepository,
            IConfiguration configuration)
        {
            _technicianRepository = technicianRepository;
            _customerRepository = customerRepository;
            _jwtKey = configuration["Jwt:Key"];
            _jwtIssuer = configuration["Jwt:Issuer"];
            _jwtAudience = configuration["Jwt:Audience"];
        }

        public string Login(LoginDTO loginDto)
        {
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

                return GenerateJwtToken(technician.ID.ToString(), "Technician");
            }
            else // Customer login
            {
                var customer = _customerRepository.GetCustomers()
                    .FirstOrDefault(c => c.Phonenumber == loginDto.Username && c.CPRCVR == loginDto.Password);
                if (customer == null) return null;

                return GenerateJwtToken(customer.ID.ToString(), "Customer");
            }
        }

        private string GenerateJwtToken(string userId, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string HashPassword(string password, string salt)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var saltedPassword = password + salt;
                var bytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
}