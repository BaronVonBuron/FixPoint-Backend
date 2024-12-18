using NUnit.Framework;
using Moq;
using FixPoint_Backend.Services;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using FixPoint_Backend.Controllers;

namespace FixPoint_Backend_Test.Services
{
    [TestFixture]
    public class CaseServiceTests
    {
        private Mock<ICaseRepository> _caseRepositoryMock;
        private Mock<ILogger<CaseController>> _loggerMock;
        private CaseService _caseService;

        [SetUp]
        public void SetUp()
        {
            _caseRepositoryMock = new Mock<ICaseRepository>();
            _loggerMock = new Mock<ILogger<CaseController>>();
            _caseService = new CaseService(_caseRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public void AddCase_ValidCase_ShouldCallRepository_AndLogMessage()
        {
            // Arrange
            var casee = new Case(Guid.NewGuid(), Guid.NewGuid(), "Type1", "Description", 1, 2, DateTime.Now, DateTime.Now.AddDays(5), "Notes");

            // Act
            _caseService.AddCase(casee);

            // Assert
            _caseRepositoryMock.Verify(repo => repo.AddCase(casee), Times.Once);
            // Check if LogInformation was called
            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Adding a new casee")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }
        

        [Test]
        public void GetCase_ValidId_ShouldReturnCase()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var casee = new Case(Guid.NewGuid(), Guid.NewGuid(), "Type3", "Description", 3, 1, DateTime.Now, DateTime.Now.AddDays(7), null)
            {
                ID = caseId
            };
            _caseRepositoryMock.Setup(repo => repo.GetCase(caseId)).Returns(casee);

            // Act
            var result = _caseService.GetCase(caseId);

            // Assert
            Assert.That(caseId, Is.EqualTo(result.ID));
            Assert.That("Type3", Is.EqualTo(result.Type));
            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Getting a casee")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);        }

        [Test]
        public void GetCase_InvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _caseRepositoryMock.Setup(repo => repo.GetCase(invalidId)).Returns((Case)null);

            // Act
            var result = _caseService.GetCase(invalidId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetCases_ShouldReturnAllCases()
        {
            // Arrange
            var cases = new List<Case>
            {
                new Case(Guid.NewGuid(), Guid.NewGuid(), "Type1", "Description1", 1, 2, DateTime.Now, DateTime.Now.AddDays(5), "Notes1"),
                new Case(Guid.NewGuid(), Guid.NewGuid(), "Type2", "Description2", 2, 3, DateTime.Now, DateTime.Now.AddDays(10), "Notes2")
            };
            _caseRepositoryMock.Setup(repo => repo.GetCases()).Returns(cases);

            // Act
            var result = _caseService.GetCases();

            // Assert
            Assert.That(2, Is.EqualTo(result.Count));
            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Getting all casees")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);        }

        [Test]
        public void UpdateCase_ValidCase_ShouldCallRepository()
        {
            // Arrange
            var casee = new Case(Guid.NewGuid(), Guid.NewGuid(), "Type1", "Updated Description", 1, 1, DateTime.Now, DateTime.Now.AddDays(3), null);

            // Act
            _caseService.UpdateCase(casee);

            // Assert
            _caseRepositoryMock.Verify(repo => repo.UpdateCase(casee), Times.Once);
            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Updating a casee")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);        }

        [Test]
        public void AddCase_NullCase_ShouldThrowArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => _caseService.AddCase(null!));
        }

        [Test]
        public void DeleteCase_NullCase_ShouldThrowArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _caseService.DeleteCase(Guid.Empty));
        }

        [Test]
        public void UpdateCase_NullCase_ShouldThrowArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => _caseService.UpdateCase(null!));
        }
    }
}