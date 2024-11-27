using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;
using FixPoint_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FixPoint_Backend_Test.Controllers;

[TestFixture]
[TestOf(typeof(MessageController))]
public class MessageControllerTest
{

        private Mock<IMessageRepository> _messageRepositoryMock;
        private Mock<ILogger<MessageController>> _loggerMock;
        private MessageService _messageService;
        private MessageController _messageController;

        [SetUp]
        public void SetUp()
        {
            _messageRepositoryMock = new Mock<IMessageRepository>();
            _loggerMock = new Mock<ILogger<MessageController>>();
            _messageService = new MessageService(_messageRepositoryMock.Object, _loggerMock.Object);
            _messageController = new MessageController(_messageService);
        }

        [Test]
        public void AddMessage_ValidMessage_ReturnsOk()
        {
            // Arrange
            var message = new Message(Guid.NewGuid(), Guid.NewGuid(), null, "Test message", DateTime.Now);

            // Act
            var result = _messageController.AddMessage(message) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value.ToString(), Does.Contain("added"));

            // Verify that the repository method was called
            _messageRepositoryMock.Verify(repo => repo.AddMessage(message), Times.Once);
        }

        [Test]
        public void GetMessageById_ValidId_ReturnsOk()
        {
            // Arrange
            var messageId = Guid.NewGuid();
            var message = new Message(Guid.NewGuid(), Guid.NewGuid(), null, "Test message", DateTime.Now) { ID = messageId };
            _messageRepositoryMock.Setup(repo => repo.GetMessage(messageId)).Returns(message);

            // Act
            var result = _messageController.GetMessageById(messageId) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value.ToString(), Does.Contain("retrieved"));
        }

        [Test]
        public void GetMessageById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _messageRepositoryMock.Setup(repo => repo.GetMessage(invalidId)).Returns((Message)null);

            // Act
            var result = _messageController.GetMessageById(invalidId) as NotFoundObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            Assert.That(result.Value.ToString(), Does.Contain("not found"));
        }

        [Test]
        public void GetMessages_ReturnsOk()
        {
            // Arrange
            var messages = new List<Message>
            {
                new Message(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Message 1", DateTime.Now),
                new Message(Guid.NewGuid(), Guid.NewGuid(), null, "Message 2", DateTime.Now)
            };
            _messageRepositoryMock.Setup(repo => repo.GetMessages()).Returns(messages);

            // Act
            var result = _messageController.GetMessages() as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value.ToString(), Does.Contain("retrieved"));
        }

        [Test]
        public void GetMessages_NoMessagesFound_ReturnsNotFound()
        {
            // Arrange
            _messageRepositoryMock.Setup(repo => repo.GetMessages()).Returns(new List<Message>());

            // Act
            var result = _messageController.GetMessages() as NotFoundObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            Assert.That(result.Value.ToString(), Does.Contain("No messages found"));
        }
}