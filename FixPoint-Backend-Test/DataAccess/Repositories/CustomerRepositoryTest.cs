using FixPoint_Backend.DataAccess;
using FixPoint_Backend.DataAccess.Repositories;
using FixPoint_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixPoint_Backend_Test.DataAccess.Repositories;

[TestFixture]
[TestOf(typeof(CustomerRepository))]
public class CustomerRepositoryTest
{

    private DbContextOptions<AppDbContext> _dbContextOptions;
        private AppDbContext _dbContext;
        private CustomerRepository _customerRepository;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(_dbContextOptions);
            _customerRepository = new CustomerRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public void AddCustomer_ValidCustomer_CustomerIsSaved()
        {
            // Arrange
            var customer = new Customer("John Doe", "john.doe@example.com", "1234567890", "123456-7890");

            // Act
            _customerRepository.AddCustomer(customer);

            // Assert
            var savedCustomer = _dbContext.Customers.FirstOrDefault(c => c.ID == customer.ID);
            Assert.That(savedCustomer, Is.Not.Null);
            Assert.That(savedCustomer.Name, Is.EqualTo("John Doe"));
            Assert.That(savedCustomer.Email, Is.EqualTo("john.doe@example.com"));
        }

        [Test]
        public void DeleteCustomer_ValidCustomer_CustomerIsDeleted()
        {
            // Arrange
            var customer = new Customer("Jane Doe", "jane.doe@example.com", "0987654321", "987654-3210");
            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();

            // Act
            _customerRepository.DeleteCustomer(customer);

            // Assert
            var deletedCustomer = _dbContext.Customers.FirstOrDefault(c => c.ID == customer.ID);
            Assert.That(deletedCustomer, Is.Null);
        }

        [Test]
        public void GetCustomer_ValidId_ReturnsCorrectCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer("Alice", "alice@example.com", "5551234567", "111111-1111") { ID = customerId };
            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();

            // Act
            var retrievedCustomer = _customerRepository.GetCustomer(customerId);

            // Assert
            Assert.That(retrievedCustomer, Is.Not.Null);
            Assert.That(retrievedCustomer.ID, Is.EqualTo(customerId));
            Assert.That(retrievedCustomer.Name, Is.EqualTo("Alice"));
        }

        [Test]
        public void GetCustomer_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var retrievedCustomer = _customerRepository.GetCustomer(invalidId);

            // Assert
            Assert.That(retrievedCustomer, Is.Null);
        }

        [Test]
        public void GetCustomers_WhenCustomersExist_ReturnsAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer("Customer 1", "cust1@example.com", "5551234", "123456-7890"),
                new Customer("Customer 2", "cust2@example.com", "5555678", "987654-3210")
            };

            _dbContext.Customers.AddRange(customers);
            _dbContext.SaveChanges();

            // Act
            var retrievedCustomers = _customerRepository.GetCustomers();

            // Assert
            Assert.That(retrievedCustomers.Count, Is.EqualTo(2));
            Assert.That(retrievedCustomers.Any(c => c.Name == "Customer 1"));
            Assert.That(retrievedCustomers.Any(c => c.Name == "Customer 2"));
        }

        [Test]
        public void GetCustomers_WhenNoCustomersExist_ReturnsEmptyList()
        {
            // Act
            var retrievedCustomers = _customerRepository.GetCustomers();

            // Assert
            Assert.That(retrievedCustomers, Is.Empty);
        }
}