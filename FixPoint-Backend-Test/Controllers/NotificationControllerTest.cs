using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;
using FixPoint_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FixPoint_Backend_Test.Controllers;

[TestFixture]
[TestOf(typeof(NotificationController))]
public class NotificationControllerTest
{

        private Mock<INotificationRepository> _notificationRepositoryMock;
        private Mock<ILogger<NotificationController>> _loggerMock;
        private NotificationService _notificationService;
        private NotificationController _notificationController;

        [SetUp]
        public void SetUp()
        {
            _notificationRepositoryMock = new Mock<INotificationRepository>();
            _loggerMock = new Mock<ILogger<NotificationController>>();
            _notificationService = new NotificationService(_notificationRepositoryMock.Object, _loggerMock.Object);
            _notificationController = new NotificationController(_notificationService);
        }

        [Test]
        public void AddNotification_ValidNotification_ReturnsOk()
        {
            // Arrange
            var notification = new Notification(Guid.NewGuid(), Guid.NewGuid(), null, "New notification", DateTime.Now);

            // Act
            var result = _notificationController.AddNotification(notification) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value.ToString(), Does.Contain("added"));

            // Verify that the repository method was called
            _notificationRepositoryMock.Verify(repo => repo.AddNotification(notification), Times.Once);
        }

        [Test]
        public void GetNotificationById_ValidId_ReturnsOk()
        {
            // Arrange
            var notificationId = Guid.NewGuid();
            var notification = new Notification(Guid.NewGuid(), Guid.NewGuid(), null, "Test notification", DateTime.Now) { ID = notificationId };
            _notificationRepositoryMock.Setup(repo => repo.GetNotification(notificationId)).Returns(notification);

            // Act
            var result = _notificationController.GetNotificationById(notificationId) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value.ToString(), Does.Contain("retrieved"));
        }

        [Test]
        public void GetNotificationById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _notificationRepositoryMock.Setup(repo => repo.GetNotification(invalidId)).Returns((Notification)null);

            // Act
            var result = _notificationController.GetNotificationById(invalidId) as NotFoundObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            Assert.That(result.Value.ToString(), Does.Contain("not found"));
        }

        [Test]
        public void GetNotifications_ReturnsOk()
        {
            // Arrange
            var notifications = new List<Notification>
            {
                new Notification(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Notification 1", DateTime.Now),
                new Notification(Guid.NewGuid(), Guid.NewGuid(), null, "Notification 2", DateTime.Now)
            };
            _notificationRepositoryMock.Setup(repo => repo.GetNotifications()).Returns(notifications);

            // Act
            var result = _notificationController.GetNotifications() as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value.ToString(), Does.Contain("retrieved"));
        }

        [Test]
        public void GetNotifications_NoNotificationsFound_ReturnsNotFound()
        {
            // Arrange
            _notificationRepositoryMock.Setup(repo => repo.GetNotifications()).Returns(new List<Notification>());

            // Act
            var result = _notificationController.GetNotifications() as NotFoundObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            Assert.That(result.Value.ToString(), Does.Contain("No notifications found"));
        }
}