using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;
using FixPoint_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FixPoint_Backend_Test.Controllers;

[TestFixture]
[TestOf(typeof(TechnicianController))]
public class TechnicianControllerTest
{

        private Mock<ITechnicianRepository> _technicianRepositoryMock;
        private Mock<ILogger<TechnicianController>> _loggerMock;
        private TechnicianService _technicianService;
        private TechnicianController _technicianController;

        [SetUp]
        public void SetUp()
        {
            _technicianRepositoryMock = new Mock<ITechnicianRepository>();
            _loggerMock = new Mock<ILogger<TechnicianController>>();
            _technicianService = new TechnicianService(_technicianRepositoryMock.Object, _loggerMock.Object);
            _technicianController = new TechnicianController(_technicianService);
        }

        [Test]
        public void GetTechnicianById_ValidId_ReturnsOk()
        {
            // Arrange
            var technicianId = Guid.NewGuid();
            var technician = new Technician("John Doe", "john.doe@example.com", "randomSalt", "hashedPassword") { ID = technicianId };
            _technicianRepositoryMock.Setup(repo => repo.GetTechnician(technicianId)).Returns(technician);

            // Act
            var result = _technicianController.GetTechnicianById(technicianId) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value.ToString(), Does.Contain("retrieved"));
        }

        [Test]
        public void GetTechnicianById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _technicianRepositoryMock.Setup(repo => repo.GetTechnician(invalidId)).Returns((Technician)null);

            // Act
            var result = _technicianController.GetTechnicianById(invalidId) as NotFoundObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            Assert.That(result.Value.ToString(), Does.Contain("Technician not found"));
        }

        [Test]
        public void GetTechnicians_ReturnsOk()
        {
            // Arrange
            var technicians = new List<Technician>
            {
                new Technician("Technician 1", "tech1@example.com", "salt1", "password1"),
                new Technician("Technician 2", "tech2@example.com", "salt2", "password2")
            };
            _technicianRepositoryMock.Setup(repo => repo.GetTechnicians()).Returns(technicians);

            // Act
            var result = _technicianController.GetTechnicians() as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value.ToString(), Does.Contain("retrieved"));
        }

        [Test]
        public void GetTechnicians_NoTechniciansFound_ReturnsNotFound()
        {
            // Arrange
            _technicianRepositoryMock.Setup(repo => repo.GetTechnicians()).Returns(new List<Technician>());

            // Act
            var result = _technicianController.GetTechnicians() as NotFoundObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            Assert.That(result.Value.ToString(), Does.Contain("No technicians found"));
        }
}