using FixPoint_Backend.DataAccess;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;
using FixPoint_Backend.Services;
using Moq;

namespace FixPoint_Backend_Test.Services;

[TestFixture]
[TestOf(typeof(AuthService))]
public class AuthServiceTest
{

        private Mock<ITechnicianRepository> _technicianRepositoryMock;
        private Mock<ICustomerRepository> _customerRepositoryMock;
        private AuthService _authService;

        [SetUp]
        public void SetUp()
        {
            _technicianRepositoryMock = new Mock<ITechnicianRepository>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _authService = new AuthService(_technicianRepositoryMock.Object, _customerRepositoryMock.Object);
        }

        [Test]
        public void Login_TechnicianValidCredentials_ReturnsToken()
        {
            // Arrange
            var salt = "randomSalt";
            var password = "password123";
            var hashedPassword = _authService.HashPassword(password, salt);
            var technician = new Technician("John Tech", "john.tech@example.com", salt, hashedPassword);
            _technicianRepositoryMock.Setup(repo => repo.GetTechnicians())
                .Returns(new List<Technician> { technician });

            var loginDto = new LoginDTO { Username = "john.tech@example.com", Password = password };

            // Act
            var result = _authService.Login(loginDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo("mockedToken"));
        }

        [Test]
        public void Login_TechnicianInvalidPassword_ReturnsNull()
        {
            // Arrange
            var salt = "randomSalt";
            var technician = new Technician("John Tech", "john.tech@example.com", salt, "hashedPassword123");
            _technicianRepositoryMock.Setup(repo => repo.GetTechnicians())
                .Returns(new List<Technician> { technician });

            var loginDto = new LoginDTO { Username = "john.tech@example.com", Password = "wrongPassword" };

            // Act
            var result = _authService.Login(loginDto);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Login_TechnicianInvalidEmail_ReturnsNull()
        {
            // Arrange
            _technicianRepositoryMock.Setup(repo => repo.GetTechnicians())
                .Returns(new List<Technician>());

            var loginDto = new LoginDTO { Username = "invalid.tech@example.com", Password = "password123" };

            // Act
            var result = _authService.Login(loginDto);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Login_CustomerValidCredentials_ReturnsToken()
        {
            // Arrange
            var customer = new Customer("Jane Customer", "jane.customer@example.com", "1234567890", "123456-7890");
            _customerRepositoryMock.Setup(repo => repo.GetCustomers())
                .Returns(new List<Customer> { customer });

            var loginDto = new LoginDTO { Username = "1234567890", Password = "123456-7890" };

            // Act
            var result = _authService.Login(loginDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo("mockedToken"));
        }

        [Test]
        public void Login_CustomerInvalidPhoneNumber_ReturnsNull()
        {
            // Arrange
            _customerRepositoryMock.Setup(repo => repo.GetCustomers())
                .Returns(new List<Customer>());

            var loginDto = new LoginDTO { Username = "invalidPhoneNumber", Password = "123456-7890" };

            // Act
            var result = _authService.Login(loginDto);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Login_CustomerInvalidPassword_ReturnsNull()
        {
            // Arrange
            var customer = new Customer("Jane Customer", "jane.customer@example.com", "1234567890", "123456-7890");
            _customerRepositoryMock.Setup(repo => repo.GetCustomers())
                .Returns(new List<Customer> { customer });

            var loginDto = new LoginDTO { Username = "1234567890", Password = "wrongPassword" };

            // Act
            var result = _authService.Login(loginDto);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Login_EmptyUsername_ReturnsNull()
        {
            // Arrange
            var loginDto = new LoginDTO { Username = "", Password = "password123" };

            // Act
            var result = _authService.Login(loginDto);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Login_EmptyPassword_ReturnsNull()
        {
            // Arrange
            var loginDto = new LoginDTO { Username = "john.tech@example.com", Password = "" };

            // Act
            var result = _authService.Login(loginDto);

            // Assert
            Assert.That(result, Is.Null);
        }
}