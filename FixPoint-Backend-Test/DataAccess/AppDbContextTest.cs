using FixPoint_Backend.DataAccess;
using FixPoint_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixPoint_Backend_Test.DataAccess;

[TestFixture]
[TestOf(typeof(AppDbContext))]
public class AppDbContextTest
{
    private DbContextOptions<AppDbContext> _dbContextOptions;
        private AppDbContext _dbContext;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(_dbContextOptions);
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
            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();

            // Assert
            var savedCustomer = _dbContext.Customers.FirstOrDefault(c => c.ID == customer.ID);
            Assert.That(savedCustomer, Is.Not.Null);
            Assert.That(savedCustomer.Name, Is.EqualTo("John Doe"));
        }

        [Test]
        public void AddCase_ValidCase_CaseIsSaved()
        {
            // Arrange
            var casee = new Case(Guid.NewGuid(), Guid.NewGuid(), "Type", "Description", 1, 2, DateTime.Now, DateTime.Now.AddDays(5), "Notes");

            // Act
            _dbContext.Cases.Add(casee);
            _dbContext.SaveChanges();

            // Assert
            var savedCase = _dbContext.Cases.FirstOrDefault(c => c.ID == casee.ID);
            Assert.That(savedCase, Is.Not.Null);
            Assert.That(savedCase.Type, Is.EqualTo("Type"));
        }

        [Test]
        public void AddMessage_ValidMessage_MessageIsSaved()
        {
            // Arrange
            var message = new Message(Guid.NewGuid(), Guid.NewGuid(), null, "Test message", DateTime.Now);

            // Act
            _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();

            // Assert
            var savedMessage = _dbContext.Messages.FirstOrDefault(m => m.ID == message.ID);
            Assert.That(savedMessage, Is.Not.Null);
            Assert.That(savedMessage.Text, Is.EqualTo("Test message"));
        }

        [Test]
        public void AddTechnician_ValidTechnician_TechnicianIsSaved()
        {
            // Arrange
            var technician = new Technician("John Tech", "tech@example.com", "salt123", "password123");

            // Act
            _dbContext.Technicians.Add(technician);
            _dbContext.SaveChanges();

            // Assert
            var savedTechnician = _dbContext.Technicians.FirstOrDefault(t => t.ID == technician.ID);
            Assert.That(savedTechnician, Is.Not.Null);
            Assert.That(savedTechnician.Name, Is.EqualTo("John Tech"));
        }

        [Test]
        public void AddNotification_ValidNotification_NotificationIsSaved()
        {
            // Arrange
            var notification = new Notification(Guid.NewGuid(), Guid.NewGuid(), null, "Test notification", DateTime.Now);

            // Act
            _dbContext.Notifications.Add(notification);
            _dbContext.SaveChanges();

            // Assert
            var savedNotification = _dbContext.Notifications.FirstOrDefault(n => n.ID == notification.ID);
            Assert.That(savedNotification, Is.Not.Null);
            Assert.That(savedNotification.Text, Is.EqualTo("Test notification"));
        }

        [Test]
        public void OnModelCreating_EnsuresCustomerConstraints()
        {
            // Arrange
            var customer = new Customer(null, null, null, null);

            // Act
            _dbContext.Customers.Add(customer);

            // Assert
            var ex = Assert.Throws<DbUpdateException>(() => _dbContext.SaveChanges());
            Assert.That(ex, Is.Not.Null);
        }
}