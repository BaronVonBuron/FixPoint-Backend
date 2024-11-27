using FixPoint_Backend.DataAccess;
using FixPoint_Backend.DataAccess.Repositories;
using FixPoint_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixPoint_Backend_Test.DataAccess.Repositories;

[TestFixture]
[TestOf(typeof(NotificationRepository))]
public class NotificationRepositoryTest
{

    private DbContextOptions<AppDbContext> _dbContextOptions;
        private AppDbContext _dbContext;
        private NotificationRepository _notificationRepository;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(_dbContextOptions);
            _notificationRepository = new NotificationRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public void AddNotification_ValidNotification_NotificationIsSaved()
        {
            // Arrange
            var notification = new Notification(Guid.NewGuid(), Guid.NewGuid(), null, "New notification", DateTime.Now);

            // Act
            _notificationRepository.AddNotification(notification);

            // Assert
            var savedNotification = _dbContext.Notifications.FirstOrDefault(n => n.ID == notification.ID);
            Assert.That(savedNotification, Is.Not.Null);
            Assert.That(savedNotification.Text, Is.EqualTo("New notification"));
        }

        [Test]
        public void GetNotification_ValidId_ReturnsCorrectNotification()
        {
            // Arrange
            var notificationId = Guid.NewGuid();
            var notification = new Notification(Guid.NewGuid(), Guid.NewGuid(), null, "Test notification", DateTime.Now) { ID = notificationId };
            _dbContext.Notifications.Add(notification);
            _dbContext.SaveChanges();

            // Act
            var retrievedNotification = _notificationRepository.GetNotification(notificationId);

            // Assert
            Assert.That(retrievedNotification, Is.Not.Null);
            Assert.That(retrievedNotification.ID, Is.EqualTo(notificationId));
            Assert.That(retrievedNotification.Text, Is.EqualTo("Test notification"));
        }

        [Test]
        public void GetNotification_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var retrievedNotification = _notificationRepository.GetNotification(invalidId);

            // Assert
            Assert.That(retrievedNotification, Is.Null);
        }

        [Test]
        public void GetNotifications_WhenNotificationsExist_ReturnsAllNotifications()
        {
            // Arrange
            var notifications = new List<Notification>
            {
                new Notification(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Notification 1", DateTime.Now),
                new Notification(Guid.NewGuid(), Guid.NewGuid(), null, "Notification 2", DateTime.Now)
            };

            _dbContext.Notifications.AddRange(notifications);
            _dbContext.SaveChanges();

            // Act
            var retrievedNotifications = _notificationRepository.GetNotifications();

            // Assert
            Assert.That(retrievedNotifications.Count, Is.EqualTo(2));
            Assert.That(retrievedNotifications.Any(n => n.Text == "Notification 1"));
            Assert.That(retrievedNotifications.Any(n => n.Text == "Notification 2"));
        }

        [Test]
        public void GetNotifications_WhenNoNotificationsExist_ReturnsEmptyList()
        {
            // Act
            var retrievedNotifications = _notificationRepository.GetNotifications();

            // Assert
            Assert.That(retrievedNotifications, Is.Empty);
        }
}