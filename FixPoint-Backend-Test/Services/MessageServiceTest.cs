using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;
using FixPoint_Backend.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FixPoint_Backend_Test.Services;

[TestFixture]
[TestOf(typeof(MessageService))]
public class MessageServiceTest
{

    private Mock<IMessageRepository> _messageRepositoryMock;
        private Mock<ILogger<MessageController>> _loggerMock;
        private MessageService _messageService;

        [SetUp]
        public void SetUp()
        {
            _messageRepositoryMock = new Mock<IMessageRepository>();
            _loggerMock = new Mock<ILogger<MessageController>>();
            _messageService = new MessageService(_messageRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public void AddMessage_ValidMessage_ShouldCallRepository()
        {
            // Arrange
            var message = new Message(Guid.NewGuid(), Guid.NewGuid(), null, "Test message", DateTime.Now);

            // Act
            _messageService.AddMessage(message);

            // Assert
            _messageRepositoryMock.Verify(repo => repo.AddMessage(message), Times.Once);
            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Adding a new message")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public void GetMessage_ValidId_ShouldReturnMessage()
        {
            // Arrange
            var messageId = Guid.NewGuid();
            var message = new Message(Guid.NewGuid(), Guid.NewGuid(), null, "Test message", DateTime.Now) { ID = messageId };
            _messageRepositoryMock.Setup(repo => repo.GetMessage(messageId)).Returns(message);

            // Act
            var result = _messageService.GetMessage(messageId);

            // Assert
            Assert.That(result.ID, Is.EqualTo(messageId));
            Assert.That(result.Text, Is.EqualTo("Test message"));
            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Getting a message by id")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public void GetMessage_InvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _messageRepositoryMock.Setup(repo => repo.GetMessage(invalidId)).Returns((Message)null);

            // Act
            var result = _messageService.GetMessage(invalidId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetMessages_ShouldReturnAllMessages()
        {
            // Arrange
            var messages = new List<Message>
            {
                new Message(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Message 1", DateTime.Now),
                new Message(Guid.NewGuid(), null, Guid.NewGuid(), "Message 2", DateTime.Now)
            };
            _messageRepositoryMock.Setup(repo => repo.GetMessages()).Returns(messages);

            // Act
            var result = _messageService.GetMessages();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Getting all messages")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public void AddMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => _messageService.AddMessage(null));
        }
}