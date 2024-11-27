using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;
using FixPoint_Backend.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FixPoint_Backend_Test.Services;

[TestFixture]
[TestOf(typeof(NotificationService))]
public class NotificationServiceTest
{

    private Mock<INotificationRepository> _notificationRepositoryMock;
        private Mock<ILogger<NotificationController>> _loggerMock;
        private NotificationService _notificationService;

        [SetUp]
        public void SetUp()
        {
            _notificationRepositoryMock = new Mock<INotificationRepository>();
            _loggerMock = new Mock<ILogger<NotificationController>>();
            _notificationService = new NotificationService(_notificationRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public void AddNotification_ValidNotification_ShouldCallRepository()
        {
            // Arrange
            var notification = new Notification(Guid.NewGuid(), Guid.NewGuid(), null, "New notification", DateTime.Now);

            // Act
            _notificationService.AddNotification(notification);

            // Assert
            _notificationRepositoryMock.Verify(repo => repo.AddNotification(notification), Times.Once);
            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Adding a new notification")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public void GetNotification_ValidId_ShouldReturnNotification()
        {
            // Arrange
            var notificationId = Guid.NewGuid();
            var notification = new Notification(Guid.NewGuid(), Guid.NewGuid(), null, "Test notification", DateTime.Now) { ID = notificationId };
            _notificationRepositoryMock.Setup(repo => repo.GetNotification(notificationId)).Returns(notification);

            // Act
            var result = _notificationService.GetNotification(notificationId);

            // Assert
            Assert.That(result.ID, Is.EqualTo(notificationId));
            Assert.That(result.Text, Is.EqualTo("Test notification"));
            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Getting a notification by id")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public void GetNotification_InvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _notificationRepositoryMock.Setup(repo => repo.GetNotification(invalidId)).Returns((Notification)null);

            // Act
            var result = _notificationService.GetNotification(invalidId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetNotifications_ShouldReturnAllNotifications()
        {
            // Arrange
            var notifications = new List<Notification>
            {
                new Notification(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Notification 1", DateTime.Now),
                new Notification(Guid.NewGuid(), null, Guid.NewGuid(), "Notification 2", DateTime.Now)
            };
            _notificationRepositoryMock.Setup(repo => repo.GetNotifications()).Returns(notifications);

            // Act
            var result = _notificationService.GetNotifications();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Getting all notifications")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public void AddNotification_NullNotification_ShouldThrowArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => _notificationService.AddNotification(null));
        }
}