using FixPoint_Backend.DataAccess;
using FixPoint_Backend.DataAccess.Repositories;
using FixPoint_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixPoint_Backend_Test.DataAccess.Repositories;

[TestFixture]
[TestOf(typeof(CaseRepository))]
public class CaseRepositoryTest
{
        private DbContextOptions<AppDbContext> _dbContextOptions;
        private AppDbContext _dbContext;
        private CaseRepository _caseRepository;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(_dbContextOptions);
            _caseRepository = new CaseRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public void AddCase_ValidCase_CaseIsSaved()
        {
            // Arrange
            var casee = new Case(Guid.NewGuid(), Guid.NewGuid(), "Type", "Description", 1, 2, DateTime.Now, DateTime.Now.AddDays(5), "Notes");

            // Act
            _caseRepository.AddCase(casee);

            // Assert
            var savedCase = _dbContext.Cases.FirstOrDefault(c => c.ID == casee.ID);
            Assert.That(savedCase, Is.Not.Null);
            Assert.That(savedCase.Type, Is.EqualTo("Type"));
        }

        [Test]
        public void DeleteCase_ValidCase_CaseIsDeleted()
        {
            // Arrange
            var casee = new Case(Guid.NewGuid(), Guid.NewGuid(), "Type", "Description", 1, 2, DateTime.Now, DateTime.Now.AddDays(5), "Notes");
            _dbContext.Cases.Add(casee);
            _dbContext.SaveChanges();

            // Act
            _caseRepository.DeleteCase(casee);

            // Assert
            var deletedCase = _dbContext.Cases.FirstOrDefault(c => c.ID == casee.ID);
            Assert.That(deletedCase, Is.Null);
        }

        [Test]
        public void GetCase_ValidId_ReturnsCorrectCase()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var casee = new Case(Guid.NewGuid(), Guid.NewGuid(), "Type", "Description", 1, 2, DateTime.Now, DateTime.Now.AddDays(5), "Notes") { ID = caseId };
            _dbContext.Cases.Add(casee);
            _dbContext.SaveChanges();

            // Act
            var retrievedCase = _caseRepository.GetCase(caseId);

            // Assert
            Assert.That(retrievedCase, Is.Not.Null);
            Assert.That(retrievedCase.ID, Is.EqualTo(caseId));
            Assert.That(retrievedCase.Type, Is.EqualTo("Type"));
        }

        [Test]
        public void GetCase_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var retrievedCase = _caseRepository.GetCase(invalidId);

            // Assert
            Assert.That(retrievedCase, Is.Null);
        }

        [Test]
        public void GetCases_WhenCasesExist_ReturnsAllCases()
        {
            // Arrange
            var cases = new List<Case>
            {
                new Case(Guid.NewGuid(), Guid.NewGuid(), "Type1", "Description1", 1, 2, DateTime.Now, DateTime.Now.AddDays(5), "Notes1"),
                new Case(Guid.NewGuid(), Guid.NewGuid(), "Type2", "Description2", 2, 3, DateTime.Now, DateTime.Now.AddDays(10), "Notes2")
            };

            _dbContext.Cases.AddRange(cases);
            _dbContext.SaveChanges();

            // Act
            var retrievedCases = _caseRepository.GetCases();

            // Assert
            Assert.That(retrievedCases.Count, Is.EqualTo(2));
            Assert.That(retrievedCases.Any(c => c.Type == "Type1"));
            Assert.That(retrievedCases.Any(c => c.Type == "Type2"));
        }

        [Test]
        public void GetCases_WhenNoCasesExist_ReturnsEmptyList()
        {
            // Act
            var retrievedCases = _caseRepository.GetCases();

            // Assert
            Assert.That(retrievedCases, Is.Empty);
        }

        [Test]
        public void UpdateCase_ValidCase_CaseIsUpdated()
        {
            // Arrange
            var casee = new Case(Guid.NewGuid(), Guid.NewGuid(), "Type", "Description", 1, 2, DateTime.Now, DateTime.Now.AddDays(5), "Notes");
            _dbContext.Cases.Add(casee);
            _dbContext.SaveChanges();

            // Act
            casee.Description = "Updated Description";
            _caseRepository.UpdateCase(casee);
            _dbContext.SaveChanges();

            // Assert
            var updatedCase = _dbContext.Cases.FirstOrDefault(c => c.ID == casee.ID);
            Assert.That(updatedCase, Is.Not.Null);
            Assert.That(updatedCase.Description, Is.EqualTo("Updated Description"));
        }
}