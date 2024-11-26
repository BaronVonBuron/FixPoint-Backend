using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;
using FixPoint_Backend.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FixPoint_Backend_Test.Services;

[TestFixture]
[TestOf(typeof(CustomerService))]
public class CustomerServiceTest
{

    private Mock<ICustomerRepository> _customerRepositoryMock;
        private Mock<ILogger<CustomerController>> _loggerMock;
        private CustomerService _customerService;

        [SetUp]
        public void SetUp()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _loggerMock = new Mock<ILogger<CustomerController>>();
            _customerService = new CustomerService(_customerRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public void AddCustomer_ValidCustomer_ShouldCallRepository()
        {
            // Arrange
            var customer = new Customer("John Doe", "john.doe@example.com", "1234567890", "123456-7890");

            // Act
            _customerService.AddCustomer(customer);

            // Assert
            _customerRepositoryMock.Verify(repo => repo.AddCustomer(customer), Times.Once);
            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Adding a new customer")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public void DeleteCustomer_ValidCustomer_ShouldCallRepository()
        {
            // Arrange
            var customer = new Customer("Jane Doe", "jane.doe@example.com", "0987654321", "987654-3210");

            // Act
            _customerService.DeleteCustomer(customer);

            // Assert
            _customerRepositoryMock.Verify(repo => repo.DeleteCustomer(customer), Times.Once);
            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Deleting a customer")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public void GetCustomer_ValidId_ShouldReturnCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer("Alice", "alice@example.com", "5551234567", "111111-1111") { ID = customerId };
            _customerRepositoryMock.Setup(repo => repo.GetCustomer(customerId)).Returns(customer);

            // Act
            var result = _customerService.GetCustomer(customerId);

            // Assert
            Assert.That(customerId, Is.EqualTo(result.ID));
            Assert.That("Alice", Is.EqualTo(result.Name));
            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Getting a customer by id")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public void GetCustomer_InvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _customerRepositoryMock.Setup(repo => repo.GetCustomer(invalidId)).Returns((Customer)null);

            // Act
            var result = _customerService.GetCustomer(invalidId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetCustomers_ShouldReturnAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer("Customer 1", "cust1@example.com", "5551234", "123456-7890"),
                new Customer("Customer 2", "cust2@example.com", "5555678", "987654-3210")
            };
            _customerRepositoryMock.Setup(repo => repo.GetCustomers()).Returns(customers);

            // Act
            var result = _customerService.GetCustomers();

            // Assert
            Assert.That(2, Is.EqualTo(result.Count));
            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Getting all customers")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public void AddCustomer_NullCustomer_ShouldThrowArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => _customerService.AddCustomer(null));
        }

        [Test]
        public void DeleteCustomer_NullCustomer_ShouldThrowArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => _customerService.DeleteCustomer(null));
        }
}