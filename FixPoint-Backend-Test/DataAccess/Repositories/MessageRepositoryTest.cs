using FixPoint_Backend.DataAccess;
using FixPoint_Backend.DataAccess.Repositories;
using FixPoint_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixPoint_Backend_Test.DataAccess.Repositories;

[TestFixture]
[TestOf(typeof(MessageRepository))]
public class MessageRepositoryTest
{

    private DbContextOptions<AppDbContext> _dbContextOptions;
        private AppDbContext _dbContext;
        private MessageRepository _messageRepository;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(_dbContextOptions);
            _messageRepository = new MessageRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public void AddMessage_ValidMessage_MessageIsSaved()
        {
            // Arrange
            var message = new Message(Guid.NewGuid(), Guid.NewGuid(), null, "Test message", DateTime.Now);

            // Act
            _messageRepository.AddMessage(message);

            // Assert
            var savedMessage = _dbContext.Messages.FirstOrDefault(m => m.ID == message.ID);
            Assert.That(savedMessage, Is.Not.Null);
            Assert.That(savedMessage.Text, Is.EqualTo("Test message"));
        }

        [Test]
        public void GetMessage_ValidId_ReturnsCorrectMessage()
        {
            // Arrange
            var messageId = Guid.NewGuid();
            var message = new Message(Guid.NewGuid(), Guid.NewGuid(), null, "Test message", DateTime.Now) { ID = messageId };
            _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();

            // Act
            var retrievedMessage = _messageRepository.GetMessage(messageId);

            // Assert
            Assert.That(retrievedMessage, Is.Not.Null);
            Assert.That(retrievedMessage.ID, Is.EqualTo(messageId));
            Assert.That(retrievedMessage.Text, Is.EqualTo("Test message"));
        }

        [Test]
        public void GetMessage_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var retrievedMessage = _messageRepository.GetMessage(invalidId);

            // Assert
            Assert.That(retrievedMessage, Is.Null);
        }

        [Test]
        public void GetMessages_WhenMessagesExist_ReturnsAllMessages()
        {
            // Arrange
            var messages = new List<Message>
            {
                new Message(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Message 1", DateTime.Now),
                new Message(Guid.NewGuid(), null, Guid.NewGuid(), "Message 2", DateTime.Now)
            };

            _dbContext.Messages.AddRange(messages);
            _dbContext.SaveChanges();

            // Act
            var retrievedMessages = _messageRepository.GetMessages();

            // Assert
            Assert.That(retrievedMessages.Count, Is.EqualTo(2));
            Assert.That(retrievedMessages.Any(m => m.Text == "Message 1"));
            Assert.That(retrievedMessages.Any(m => m.Text == "Message 2"));
        }

        [Test]
        public void GetMessages_WhenNoMessagesExist_ReturnsEmptyList()
        {
            // Act
            var retrievedMessages = _messageRepository.GetMessages();

            // Assert
            Assert.That(retrievedMessages, Is.Empty);
        }
}