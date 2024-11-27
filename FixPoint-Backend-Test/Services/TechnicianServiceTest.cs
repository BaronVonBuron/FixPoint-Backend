using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;
using FixPoint_Backend.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FixPoint_Backend_Test.Services;

[TestFixture]
[TestOf(typeof(TechnicianService))]
public class TechnicianServiceTest
{

    [TestFixture]
    public class TechnicianServiceTests
    {
        private Mock<ITechnicianRepository> _technicianRepositoryMock;
        private Mock<ILogger<TechnicianController>> _loggerMock;
        private TechnicianService _technicianService;

        [SetUp]
        public void SetUp()
        {
            _technicianRepositoryMock = new Mock<ITechnicianRepository>();
            _loggerMock = new Mock<ILogger<TechnicianController>>();
            _technicianService = new TechnicianService(_technicianRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public void GetTechnician_ValidId_ShouldReturnTechnician()
        {
            // Arrange
            var technicianId = Guid.NewGuid();
            var technician = new Technician("John Doe", "john.doe@example.com", "randomSalt", "hashedPassword")
                { ID = technicianId };
            _technicianRepositoryMock.Setup(repo => repo.GetTechnician(technicianId)).Returns(technician);

            // Act
            var result = _technicianService.GetTechnician(technicianId);

            // Assert
            Assert.That(result.ID, Is.EqualTo(technicianId));
            Assert.That(result.Name, Is.EqualTo("John Doe"));
            Assert.That(result.Email, Is.EqualTo("john.doe@example.com"));
            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Getting a technician by id")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public void GetTechnician_InvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _technicianRepositoryMock.Setup(repo => repo.GetTechnician(invalidId)).Returns((Technician)null);

            // Act
            var result = _technicianService.GetTechnician(invalidId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetTechnicians_ShouldReturnAllTechnicians()
        {
            // Arrange
            var technicians = new List<Technician>
            {
                new Technician("Technician 1", "tech1@example.com", "salt1", "password1"),
                new Technician("Technician 2", "tech2@example.com", "salt2", "password2")
            };
            _technicianRepositoryMock.Setup(repo => repo.GetTechnicians()).Returns(technicians);

            // Act
            var result = _technicianService.GetTechnicians();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("Technician 1"));
            Assert.That(result[1].Name, Is.EqualTo("Technician 2"));
            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Getting all technicians")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}