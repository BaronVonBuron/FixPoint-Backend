using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;
using FixPoint_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FixPoint_Backend_Test.Controllers;

[TestFixture]
[TestOf(typeof(CustomerController))]
public class CustomerControllerTest
{
        private Mock<ICustomerRepository> _customerRepositoryMock;
        private Mock<ILogger<CustomerController>> _loggerMock;
        private CustomerService _customerService;
        private CustomerController _customerController;

        [SetUp]
        public void SetUp()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _loggerMock = new Mock<ILogger<CustomerController>>();
            _customerService = new CustomerService(_customerRepositoryMock.Object, _loggerMock.Object);
            _customerController = new CustomerController(_customerService);
        }

        [Test]
        public void AddCustomer_ValidCustomer_ReturnsOk()
        {
            // Arrange
            var customer = new Customer("John Doe", "john.doe@example.com", "1234567890", "123456-7890");

            // Act
            var result = _customerController.AddCustomer(customer) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value as Customer, Is.EqualTo(customer));

            // Verify that the repository method was called
            _customerRepositoryMock.Verify(repo => repo.AddCustomer(customer), Times.Once);
        }

        [Test]
        public void DeleteCustomer_ValidCustomer_ReturnsOk()
        {
            // Arrange
            var customer = new Customer("Jane Doe", "jane.doe@example.com", "0987654321", "987654-3210");

            // Act
            var result = _customerController.DeleteCustomer(customer) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value.ToString(), Does.Contain("deleted"));

            // Verify that the repository method was called
            _customerRepositoryMock.Verify(repo => repo.DeleteCustomer(customer), Times.Once);
        }

        [Test]
        public void GetCustomerById_ValidId_ReturnsOk()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer("Alice", "alice@example.com", "5551234567", "111111-1111") { ID = customerId };
            _customerRepositoryMock.Setup(repo => repo.GetCustomer(customerId)).Returns(customer);

            // Act
            var result = _customerController.GetCustomerById(customerId) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value.Equals(customer));
        }

        [Test]
        public void GetCustomerById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _customerRepositoryMock.Setup(repo => repo.GetCustomer(invalidId)).Returns((Customer)null);

            // Act
            var result = _customerController.GetCustomerById(invalidId) as NotFoundObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            Assert.That(result.Value.ToString(), Does.Contain("not found"));
        }

        [Test]
        public void GetCustomers_ReturnsOk()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer("Customer 1", "cust1@example.com", "5551234", "123456-7890"),
                new Customer("Customer 2", "cust2@example.com", "5555678", "987654-3210")
            };
            _customerRepositoryMock.Setup(repo => repo.GetCustomers()).Returns(customers);

            // Act
            var result = _customerController.GetCustomers() as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value.ToString(), Does.Contain("retrieved"));
        }

        [Test]
        public void GetCustomers_NoCustomersFound_ReturnsNotFound()
        {
            // Arrange
            _customerRepositoryMock.Setup(repo => repo.GetCustomers()).Returns(new List<Customer>());

            // Act
            var result = _customerController.GetCustomers() as NotFoundObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            Assert.That(result.Value.ToString(), Does.Contain("No customers found"));
        }
}